namespace ExpenseTracker.Application.Contracts.Reports;

public record MonthComparisonFilters
{
    public DateOnly FromMonth { get; init; }
    public DateOnly ToMonth { get; init; }
}
