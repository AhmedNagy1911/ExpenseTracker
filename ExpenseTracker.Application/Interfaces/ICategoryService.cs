using ExpenseTracker.Application.Contracts.Categories;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetAllAsync(string userId, TransactionType? type = null, CancellationToken cancellationToken = default);
}
