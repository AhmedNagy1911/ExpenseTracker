using ExpenseTracker.Application.Contracts.Reports;

namespace ExpenseTracker.Application.Interfaces;

public interface IReportService
{
    Task<MonthlySummaryResponse> GetMonthlySummaryAsync(string userId, int year, int month, CancellationToken cancellationToken = default);
    Task<IEnumerable<CategoryBreakdownResponse>> GetCategoryBreakdownAsync(string userId, int year, int month, CancellationToken cancellationToken = default);
    Task<IEnumerable<MonthComparisonResponse>> GetMonthComparisonAsync(string userId, MonthComparisonFilters filters, CancellationToken cancellationToken = default);
}
