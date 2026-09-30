using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Contracts.RecurringTransactions;

public record RecurringTransactionResponse(
    Guid Id,
    Guid CategoryId,
    string CategoryName,
    decimal Amount,
    TransactionType Type,
    string? Description,
    RecurringFrequency Frequency,
    DateTime StartDate,
    DateTime NextRunDate,
    bool IsActive
);