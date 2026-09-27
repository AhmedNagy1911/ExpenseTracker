using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Contracts.Categories;

public record CategoryResponse(
    Guid Id,
    string Name,
    TransactionType Type,
    DateTime CreatedAt
);
