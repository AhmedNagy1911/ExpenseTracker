using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Common.Errors;

public record BudgetErrors
{
    public static readonly Error BudgetNotFound =
        new("Budget.BudgetNotFound", "Budget is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedBudget =
        new("Budget.DuplicatedBudget", "A budget for this category and period already exists", StatusCodes.Status409Conflict);

    public static readonly Error InvalidCategoryType =
        new("Budget.InvalidCategoryType", "Budgets can only be set for Expense categories", StatusCodes.Status400BadRequest);
}