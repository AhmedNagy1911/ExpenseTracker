using ExpenseTracker.Application.Common.Models;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Contracts.Transactions;

public record TransactionFilters : RequestFilters
{
    public TransactionType? Type { get; init; }
    public Guid? CategoryId { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public decimal? MinAmount { get; init; }
    public decimal? MaxAmount { get; init; }
}
