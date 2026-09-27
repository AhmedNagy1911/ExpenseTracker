using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Common;

public interface IApplicationDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<RecurringTransaction> RecurringTransactions { get; }
    DbSet<Budget> Budgets { get; }
    DbSet<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}