using ExpenseTracker.Application.Common.Models;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Contracts.RecurringTransactions;

public record RecurringTransactionFilters : RequestFilters
{
    public TransactionType? Type { get; init; }
    public Guid? CategoryId { get; init; }
    public bool? IsActive { get; init; }
}