using ExpenseTracker.Application.Common.Models;
using ExpenseTracker.Application.Common.Results;
using ExpenseTracker.Application.Contracts.Transactions;

namespace ExpenseTracker.Application.Interfaces;

public interface ITransactionService
{
    Task<PaginatedList<TransactionResponse>> GetAllAsync(string userId, TransactionFilters filters, CancellationToken cancellationToken = default);
    Task<Result<TransactionResponse>> GetAsync(string userId, Guid id, CancellationToken cancellationToken = default);
    Task<Result<TransactionResponse>> AddAsync(string userId, TransactionRequest request, CancellationToken cancellationToken = default);

}
