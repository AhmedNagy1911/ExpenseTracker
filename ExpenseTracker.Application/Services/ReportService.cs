using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Contracts.Reports;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Services;

public class ReportService(IApplicationDbContext context) : IReportService
{
    private readonly IApplicationDbContext _context = context;

    public async Task<MonthlySummaryResponse> GetMonthlySummaryAsync(string userId, int year, int month, CancellationToken cancellationToken = default)
    {
        var (start, end) = GetPeriod(year, month);

        var totals = await _context.Transactions
            .Where(t => t.UserId == userId && t.Date >= start && t.Date < end)
            .GroupBy(t => t.Type)
            .Select(g => new { Type = g.Key, Total = g.Sum(x => x.Amount) })
            .ToListAsync(cancellationToken);

        var income = totals.SingleOrDefault(x => x.Type == TransactionType.Income)?.Total ?? 0;
        var expenses = totals.SingleOrDefault(x => x.Type == TransactionType.Expense)?.Total ?? 0;

        return new MonthlySummaryResponse(year, month, income, expenses, income - expenses);
    }

    public async Task<IEnumerable<CategoryBreakdownResponse>> GetCategoryBreakdownAsync(string userId, int year, int month, CancellationToken cancellationToken = default)
    {
        var (start, end) = GetPeriod(year, month);

        return await (
            from transaction in _context.Transactions
            join category in _context.Categories
                on transaction.CategoryId equals category.Id
            where transaction.UserId == userId
                  && transaction.Type == TransactionType.Expense
                  && transaction.Date >= start
                  && transaction.Date < end
            group transaction by new
            {
                transaction.CategoryId,
                CategoryName = category.Name
            }
            into g
            select new
            {
                g.Key.CategoryId,
                g.Key.CategoryName,
                Amount = g.Sum(x => x.Amount)
            }
        )
        .OrderByDescending(x => x.Amount)
        .Select(x => new CategoryBreakdownResponse(
            x.CategoryId,
            x.CategoryName,
            x.Amount))
        .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MonthComparisonResponse>> GetMonthComparisonAsync(string userId, MonthComparisonFilters filters, CancellationToken cancellationToken = default)
    {
        var start = new DateTime(filters.FromMonth.Year, filters.FromMonth.Month, 1);
        var end = new DateTime(filters.ToMonth.Year, filters.ToMonth.Month, 1).AddMonths(1);

        var totals = await _context.Transactions
            .Where(t => t.UserId == userId && t.Date >= start && t.Date < end)
            .GroupBy(t => new { t.Date.Year, t.Date.Month, t.Type })
            .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Type, Total = g.Sum(x => x.Amount) })
            .ToListAsync(cancellationToken);

        var result = new List<MonthComparisonResponse>();

        for (var cursor = start; cursor < end; cursor = cursor.AddMonths(1))
        {
            var income = totals.SingleOrDefault(x => x.Year == cursor.Year && x.Month == cursor.Month && x.Type == TransactionType.Income)?.Total ?? 0;
            var expenses = totals.SingleOrDefault(x => x.Year == cursor.Year && x.Month == cursor.Month && x.Type == TransactionType.Expense)?.Total ?? 0;

            result.Add(new MonthComparisonResponse(cursor.Year, cursor.Month, income, expenses, income - expenses));
        }
        return result;
    }
    private static (DateTime Start, DateTime End) GetPeriod(int year, int month)
    {
        var start = new DateTime(year, month, 1);
        return (start, start.AddMonths(1));
    }
}
