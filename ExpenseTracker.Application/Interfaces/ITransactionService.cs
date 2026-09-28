using ExpenseTracker.Application.Common.Models;
using ExpenseTracker.Application.Contracts.Transactions;

namespace ExpenseTracker.Application.Interfaces;

public interface ITransactionService
{
    Task<PaginatedList<TransactionResponse>> GetAllAsync(string userId, TransactionFilters filters, CancellationToken cancellationToken = default);
}
