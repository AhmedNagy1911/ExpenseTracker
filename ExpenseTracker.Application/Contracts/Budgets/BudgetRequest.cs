namespace ExpenseTracker.Application.Contracts.Budgets;

public record BudgetRequest(
    Guid CategoryId,
    decimal Amount,
    int Month,
    int Year
);