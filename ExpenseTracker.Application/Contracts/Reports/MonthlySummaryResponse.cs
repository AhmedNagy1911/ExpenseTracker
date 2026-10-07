namespace ExpenseTracker.Application.Contracts.Reports;

public record MonthlySummaryResponse(
    int Year,
    int Month,
    decimal TotalIncome,
    decimal TotalExpenses,
    decimal Net
);