namespace ExpenseTracker.Application.Contracts.Transactions;

public record TransactionRequest(
    Guid CategoryId,
    decimal Amount,
    DateTime Date,
    string? Description
);