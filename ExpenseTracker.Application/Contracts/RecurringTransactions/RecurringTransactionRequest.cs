namespace ExpenseTracker.Application.Contracts.RecurringTransactions;

public record RecurringTransactionRequest(
    Guid CategoryId,
    decimal Amount,
    string? Description,
    DateTime StartDate
);