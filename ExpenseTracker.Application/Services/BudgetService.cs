using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Results;
using ExpenseTracker.Application.Contracts.Budgets;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Services;

public class BudgetService(IApplicationDbContext context) : IBudgetService
{
    private readonly IApplicationDbContext _context = context;

    public async Task<IEnumerable<BudgetResponse>> GetAllAsync(string userId, BudgetFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.Budgets
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        if (filters.Month.HasValue)
            query = query.Where(x => x.Month == filters.Month);

        if (filters.Year.HasValue)
            query = query.Where(x => x.Year == filters.Year);

        if (filters.CategoryId.HasValue)
            query = query.Where(x => x.CategoryId == filters.CategoryId);

        return await query
            .OrderBy(x => x.Category.Name)
            .ProjectToType<BudgetResponse>()
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<BudgetResponse>> GetAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var budget = await _context.Budgets
            .AsNoTracking()
            .Where(x => x.UserId == userId && x.Id == id)
            .ProjectToType<BudgetResponse>()
            .SingleOrDefaultAsync(cancellationToken);

        return budget is null
            ? Result.Failure<BudgetResponse>(BudgetErrors.BudgetNotFound)
            : Result.Success(budget);
    }

    public async Task<Result<BudgetResponse>> AddAsync(string userId, BudgetRequest request, CancellationToken cancellationToken = default)
    {
        var category = await _context.Categories
             .AsNoTracking()
             .SingleOrDefaultAsync(x => x.Id == request.CategoryId && x.UserId == userId, cancellationToken);

        if (category is null)
            return Result.Failure<BudgetResponse>(CategoryErrors.CategoryNotFound);

        if (category.Type != TransactionType.Expense)
            return Result.Failure<BudgetResponse>(BudgetErrors.InvalidCategoryType);

        var isDuplicated = await _context.Budgets
            .AsNoTracking()
            .AnyAsync(x =>
            x.UserId == userId &&
            x.CategoryId == request.CategoryId &&
            x.Month == request.Month &&
            x.Year == request.Year,
            cancellationToken);

        if (isDuplicated)
            return Result.Failure<BudgetResponse>(BudgetErrors.DuplicatedBudget);

        var budget = new Budget
        {
            UserId = userId,
            CategoryId = request.CategoryId,
            Month = request.Month,
            Year = request.Year,
            Amount = request.Amount
        };

        await _context.Budgets.AddAsync(budget, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new BudgetResponse(
            budget.Id, category.Id, category.Name, budget.Amount, budget.Month, budget.Year, budget.CreatedAt);

        return Result.Success(response);
    }

    public async Task<Result> UpdateAsync(string userId, Guid id, BudgetRequest request, CancellationToken cancellationToken = default)
    {
        var budget = await _context.Budgets
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);

        if (budget is null)
            return Result.Failure(BudgetErrors.BudgetNotFound);

        var category = await _context.Categories
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == request.CategoryId, cancellationToken);

        if (category is null)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        if (category.Type != TransactionType.Expense)
            return Result.Failure(BudgetErrors.InvalidCategoryType);

        var isDuplicated = await _context.Budgets.AnyAsync(x =>
            x.UserId == userId &&
            x.CategoryId == request.CategoryId &&
            x.Month == request.Month &&
            x.Year == request.Year &&
            x.Id != id,
            cancellationToken);

        if (isDuplicated)
            return Result.Failure(BudgetErrors.DuplicatedBudget);

        budget.CategoryId = category.Id;
        budget.Amount = request.Amount;
        budget.Month = request.Month;
        budget.Year = request.Year;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var budget = await _context.Budgets
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);

        if (budget is null)
            return Result.Failure(BudgetErrors.BudgetNotFound);


        await _context.Notifications
            .Where(n => n.BudgetId == id)
            .ExecuteDeleteAsync(cancellationToken);

        _context.Budgets.Remove(budget);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
