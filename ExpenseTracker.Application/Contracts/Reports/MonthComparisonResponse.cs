namespace ExpenseTracker.Application.Contracts.Reports;

public record MonthComparisonResponse(
    int Year,
    int Month,
    decimal Income,
    decimal Expenses,
    decimal Net
);