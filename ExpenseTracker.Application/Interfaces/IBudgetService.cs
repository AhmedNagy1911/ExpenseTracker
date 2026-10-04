using ExpenseTracker.Application.Common.Results;
using ExpenseTracker.Application.Contracts.Budgets;

namespace ExpenseTracker.Application.Interfaces;

public interface IBudgetService
{
    Task<IEnumerable<BudgetResponse>> GetAllAsync(string userId, BudgetFilters filters, CancellationToken cancellationToken = default);
    Task<Result<BudgetResponse>> GetAsync(string userId, Guid id, CancellationToken cancellationToken = default);
    Task<Result<BudgetResponse>> AddAsync(string userId, BudgetRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string userId, Guid id, BudgetRequest request, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default);
}
