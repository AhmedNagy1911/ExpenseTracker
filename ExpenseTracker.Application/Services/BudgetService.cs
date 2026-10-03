using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Results;
using ExpenseTracker.Application.Contracts.Budgets;
using ExpenseTracker.Application.Interfaces;
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

}
