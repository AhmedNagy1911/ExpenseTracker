using ExpenseTracker.Application.Common.Models;
using ExpenseTracker.Application.Contracts.Transactions;
using ExpenseTracker.Application.Interfaces;

namespace ExpenseTracker.Application.Services;

public class TransactionService : ITransactionService
{
    public Task<PaginatedList<TransactionResponse>> GetAllAsync(string userId, TransactionFilters filters, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
