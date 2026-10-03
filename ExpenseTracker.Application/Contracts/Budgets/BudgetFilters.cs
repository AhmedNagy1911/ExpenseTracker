namespace ExpenseTracker.Application.Contracts.Budgets;

public record BudgetFilters
{
    public int? Month { get; init; }
    public int? Year { get; init; }
    public Guid? CategoryId { get; init; }
}