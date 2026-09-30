using ExpenseTracker.Application.Common.Models;
using ExpenseTracker.Application.Common.Results;
using ExpenseTracker.Application.Contracts.RecurringTransactions;

namespace ExpenseTracker.Application.Interfaces;

public interface IRecurringTransactionService
{
    Task<PaginatedList<RecurringTransactionResponse>> GetAllAsync(string userId, RecurringTransactionFilters filters, CancellationToken cancellationToken = default);
    Task<Result<RecurringTransactionResponse>> GetAsync(string userId, Guid id, CancellationToken cancellationToken = default);
}
