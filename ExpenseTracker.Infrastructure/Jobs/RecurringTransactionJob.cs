using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Infrastructure.Jobs;

public class RecurringTransactionJob(ApplicationDbContext context, ILogger<RecurringTransactionJob> logger) : IRecurringTransactionJob
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<RecurringTransactionJob> _logger = logger;

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;

        var dueRecurring = await _context.RecurringTransactions
            .Where(x => x.IsActive && x.NextRunDate <= today)
            .ToListAsync(cancellationToken);

        if (dueRecurring.Count == 0)
        {
            _logger.LogInformation("No recurring transactions due today.");
            return;
        }

        var generatedCount = 0;

        foreach (var recurring in dueRecurring)
        {
            while (recurring.NextRunDate <= today)
            {
                var transaction = new Transaction
                {
                    UserId = recurring.UserId,
                    CategoryId = recurring.CategoryId,
                    Amount = recurring.Amount,
                    Type = recurring.Type,
                    Description = recurring.Description,
                    Date = recurring.NextRunDate,
                    RecurringTransactionId = recurring.Id
                };

                await _context.Transactions.AddAsync(transaction, cancellationToken);
                generatedCount++;

                recurring.NextRunDate = recurring.Frequency switch
                {
                    Domain.Enums.RecurringFrequency.Monthly => recurring.NextRunDate.AddMonths(1),
                    _ => recurring.NextRunDate.AddMonths(1)
                };
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Generated {Count} transactions from {RecurringCount} recurring transactions.", generatedCount, dueRecurring.Count);
    }
}