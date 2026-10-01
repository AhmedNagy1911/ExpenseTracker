namespace ExpenseTracker.Infrastructure.Jobs;

public interface IRecurringTransactionJob
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}