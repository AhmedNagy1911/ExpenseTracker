using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Models;
using ExpenseTracker.Application.Common.Results;
using ExpenseTracker.Application.Contracts.Transactions;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace ExpenseTracker.Application.Services;

public class TransactionService(IApplicationDbContext context) : ITransactionService
{
    private readonly IApplicationDbContext _context = context;

    // Whitelist: أي SortColumn برا القائمة دي بيتتجاهل (حماية من Dynamic LINQ injection)
    private static readonly Dictionary<string, string> SortableColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        ["date"] = nameof(Transaction.Date),
        ["amount"] = nameof(Transaction.Amount),
        ["createdAt"] = nameof(Transaction.CreatedAt),
        ["description"] = nameof(Transaction.Description),
        ["category"] = "Category.Name"
    };
    public async Task<PaginatedList<TransactionResponse>> GetAllAsync(string userId, TransactionFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.Transactions
          .AsNoTracking()
          .Where(x => x.UserId == userId);

        if (filters.Type.HasValue)
            query = query.Where(x => x.Type == filters.Type);

        if (filters.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == filters.CategoryId);

        if (filters.FromDate.HasValue)
            query = query.Where(x => x.Date >= filters.FromDate.Value.Date);

        if (filters.ToDate.HasValue)   // inclusive لآخر اليوم
            query = query.Where(x => x.Date < filters.ToDate.Value.Date.AddDays(1));

        if (filters.MinAmount.HasValue)
            query = query.Where(x => x.Amount >= filters.MinAmount);

        if (filters.MaxAmount.HasValue)
            query = query.Where(x => x.Amount <= filters.MaxAmount);

        if (!string.IsNullOrWhiteSpace(filters.SearchValue))
        {
            var search = filters.SearchValue.Trim();
            query = query.Where(x =>
                (x.Description != null && x.Description.Contains(search)) ||
                x.Category.Name.Contains(search));
        }

        query = ApplySorting(query, filters);

        return await PaginatedList<TransactionResponse>.CreateAsync(
            query.ProjectToType<TransactionResponse>(),
            filters.PageNumber,
            filters.PageSize,
            cancellationToken);
    }

    public async Task<Result<TransactionResponse>> GetAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Transactions
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Id == id)
            .ProjectToType<TransactionResponse>()
            .SingleOrDefaultAsync(cancellationToken);

        return transaction is null
            ? Result.Failure<TransactionResponse>(TransactionErrors.TransactionNotFound)
            : Result.Success(transaction);
    }
    public async Task<Result<TransactionResponse>> AddAsync(string userId, TransactionRequest request, CancellationToken cancellationToken = default)
    {
        // الـ Category لازم تكون بتاعة نفس اليوزر
        var category = await _context.Categories
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure<TransactionResponse>(CategoryErrors.CategoryNotFound);

        var transaction = new Transaction
        {
            UserId = userId,
            CategoryId = category.Id,
            Type = category.Type,          // النوع متاخد من الـ Category
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description
        };

        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new TransactionResponse(
            transaction.Id, category.Id, category.Name, transaction.Amount, transaction.Type,
            transaction.Description, transaction.Date, transaction.RecurringTransactionId, transaction.CreatedAt);

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(string userId, Guid id, TransactionRequest request, CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Transactions
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);

        if (transaction is null)
            return Result.Failure(TransactionErrors.TransactionNotFound);

        var category = await _context.Categories
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        transaction.CategoryId = category.Id;
        transaction.Type = category.Type;
        transaction.Amount = request.Amount;
        transaction.Date = request.Date;
        transaction.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private static IQueryable<Transaction> ApplySorting(IQueryable<Transaction> query, TransactionFilters filters)
    {
        // Default: الأحدث الأول. Id (Guid v7) كـ tie-breaker عشان الـ Pagination يبقى ثابت
        if (string.IsNullOrWhiteSpace(filters.SortColumn) ||
            !SortableColumns.TryGetValue(filters.SortColumn, out var column))
            return query.OrderByDescending(x => x.Date).ThenByDescending(x => x.Id);

        var direction = string.Equals(filters.SortDirection, "DESC", StringComparison.OrdinalIgnoreCase) ? "desc" : "asc";

        return query.OrderBy($"{column} {direction}, Id {direction}");
    }
}
