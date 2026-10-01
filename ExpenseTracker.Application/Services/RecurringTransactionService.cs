using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Models;
using ExpenseTracker.Application.Common.Results;
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

    public async Task<Result<RecurringTransactionResponse>> GetAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var recurring = await _context.RecurringTransactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Id == id)
            .ProjectToType<RecurringTransactionResponse>()
            .SingleOrDefaultAsync(cancellationToken);

        return recurring is null
            ? Result.Failure<RecurringTransactionResponse>(RecurringTransactionErrors.NotFound)
            : Result.Success(recurring);
    }

    public async Task<Result<RecurringTransactionResponse>> AddAsync(string userId, RecurringTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure<RecurringTransactionResponse>(CategoryErrors.CategoryNotFound);

        var recurring = new RecurringTransaction
        {
            UserId = userId,
            CategoryId = category.Id,
            Type = category.Type,
            Amount = request.Amount,
            Description = request.Description,
            StartDate = request.StartDate.Date,
            NextRunDate = request.StartDate.Date,
            IsActive = true
        };

        await _context.RecurringTransactions.AddAsync(recurring, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new RecurringTransactionResponse(
            recurring.Id, category.Id, category.Name, recurring.Amount, recurring.Type,
            recurring.Description, recurring.Frequency, recurring.StartDate, recurring.NextRunDate, recurring.IsActive);

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(string userId, Guid id, RecurringTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var recurring = await _context.RecurringTransactions
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);

        if (recurring is null)
            return Result.Failure(RecurringTransactionErrors.NotFound);

        var category = await _context.Categories
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        recurring.CategoryId = category.Id;
        recurring.Type = category.Type;
        recurring.Amount = request.Amount;
        recurring.Description = request.Description;


        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ToggleStatusAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var recurring = await _context.RecurringTransactions
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);

        if (recurring is null)
            return Result.Failure(RecurringTransactionErrors.NotFound);

        recurring.IsActive = !recurring.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var recurring = await _context.RecurringTransactions
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);

        if (recurring is null)
            return Result.Failure(RecurringTransactionErrors.NotFound);

        _context.RecurringTransactions.Remove(recurring);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
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
