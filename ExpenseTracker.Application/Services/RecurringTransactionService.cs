using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Models;
using ExpenseTracker.Application.Contracts.RecurringTransactions;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ExpenseTracker.Application.Services;

public class RecurringTransactionService(IApplicationDbContext context) : IRecurringTransactionService
{
    private readonly IApplicationDbContext _context = context;

    private static readonly Dictionary<string, string> SortableColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["amount"] = nameof(RecurringTransaction.Amount),
        ["nextRunDate"] = nameof(RecurringTransaction.NextRunDate),
        ["startDate"] = nameof(RecurringTransaction.StartDate),
        ["category"] = "Category.Name"
    };

    public async Task<PaginatedList<RecurringTransactionResponse>> GetAllAsync(string userId, RecurringTransactionFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.RecurringTransactions
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        if (filters.Type.HasValue)
            query = query.Where(x => x.Type == filters.Type);

        if (filters.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == filters.CategoryId);

        if (filters.IsActive.HasValue)
            query = query.Where(x => x.IsActive == filters.IsActive);

        if (!string.IsNullOrWhiteSpace(filters.SearchValue))
        {
            var search = filters.SearchValue.Trim();
            query = query.Where(x =>
                (x.Description != null && x.Description.Contains(search)) ||
                x.Category.Name.Contains(search));
        }

        query = ApplySorting(query, filters);

        return await PaginatedList<RecurringTransactionResponse>.CreateAsync(
            query.ProjectToType<RecurringTransactionResponse>(),
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);
    }


    private static IQueryable<RecurringTransaction> ApplySorting(IQueryable<RecurringTransaction> query, RecurringTransactionFilters filters)
    {
        if (string.IsNullOrWhiteSpace(filters.SortColumn) ||
            !SortableColumns.TryGetValue(filters.SortColumn, out var column))
            return query.OrderBy(x => x.NextRunDate).ThenByDescending(x => x.Id);

        var direction = string.Equals(filters.SortDirection, "DESC", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";

        return query.OrderBy($"{column} {direction}, Id {direction}");
    }
}
