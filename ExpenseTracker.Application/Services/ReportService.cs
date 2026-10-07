using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Contracts.Reports;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Services;

public class ReportService(IApplicationDbContext context) : IReportService
{
    private readonly IApplicationDbContext _context = context;

    public async Task<MonthlySummaryResponse> GetMonthlySummaryAsync(string userId, int year, int month, CancellationToken cancellationToken = default)
    {
        var (start, end) = GetPeriod(year, month);

        var totals = await _context.Transactions
            .Where(t => t.UserId == userId && t.Date >= start && t.Date < end)
            .GroupBy(t => t.Type)
            .Select(g => new { Type = g.Key, Total = g.Sum(x => x.Amount) })
            .ToListAsync(cancellationToken);

        var income = totals.SingleOrDefault(x => x.Type == TransactionType.Income)?.Total ?? 0;
        var expenses = totals.SingleOrDefault(x => x.Type == TransactionType.Expense)?.Total ?? 0;

        return new MonthlySummaryResponse(year, month, income, expenses, income - expenses);
    }



    private static (DateTime Start, DateTime End) GetPeriod(int year, int month)
    {
        var start = new DateTime(year, month, 1);
        return (start, start.AddMonths(1));
    }
}
