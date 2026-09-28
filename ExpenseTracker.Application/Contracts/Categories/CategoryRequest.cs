using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Contracts.Categories;

public record CategoryRequest(
    string Name,
    TransactionType Type
);