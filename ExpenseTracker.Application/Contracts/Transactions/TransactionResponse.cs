using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Contracts.Transactions;

public record TransactionResponse(
    Guid Id,
    Guid CategoryId,
    string CategoryName,   // Mapster بيعمل flatten من Category.Name لوحده
    decimal Amount,
    TransactionType Type,
    string? Description,
    DateTime Date,
    Guid? RecurringTransactionId,
    DateTime CreatedAt
);