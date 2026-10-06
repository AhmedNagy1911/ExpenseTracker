namespace ExpenseTracker.Infrastructure.Jobs;

public interface IBudgetCheckJob
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}