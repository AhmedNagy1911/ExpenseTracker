using ExpenseTracker.Application.Common.Results;
using ExpenseTracker.Application.Contracts.Categories;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetAllAsync(string userId, TransactionType? type = null, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> GetAsync(string userId, Guid id, CancellationToken cancellationToken = default);
    Task<Result<CategoryResponse>> AddAsync(string userId, CategoryRequest request, CancellationToken cancellationToken = default);
}
