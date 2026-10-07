namespace ExpenseTracker.Application.Contracts.Reports;

public record CategoryBreakdownResponse(
    Guid CategoryId,
    string Category,
    decimal Amount
);