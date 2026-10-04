namespace ExpenseTracker.Application.Contracts.Budgets;

public record BudgetResponse(
    Guid Id,
    Guid CategoryId,
    string CategoryName,
    decimal Amount,
    int Month,
    int Year,
    DateTime CreatedAt
);