using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Contracts.Categories;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Services;

public class CategoryService(IApplicationDbContext context) : ICategoryService
{
    private readonly IApplicationDbContext _context = context;

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync(string userId, TransactionType? type = null, CancellationToken cancellationToken = default) =>
        await _context.Categories
                .Where(x => x.UserId == userId)
                .Where(x => type == null || x.Type == type)
                .OrderBy(x => x.Name)
                .ProjectToType<CategoryResponse>()
                .ToListAsync(cancellationToken);
}
