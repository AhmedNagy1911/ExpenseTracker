using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Results;
using ExpenseTracker.Application.Contracts.Categories;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
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

    public async Task<Result<CategoryResponse>> GetAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _context.Categories
                      .Where(x => x.UserId == userId && x.Id == id)
                      .ProjectToType<CategoryResponse>()
                      .SingleOrDefaultAsync(cancellationToken);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryErrors.CategoryNotFound);

        return Result.Success(category);
    }

    public async Task<Result<CategoryResponse>> AddAsync(string userId, CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var isDuplicated = await _context.Categories
            .AnyAsync(x => x.UserId == userId && x.Name == request.Name && x.Type == request.Type, cancellationToken);

        if (isDuplicated)
            return Result.Failure<CategoryResponse>(CategoryErrors.DuplicatedCategory);

        var category = new Category
        {
            UserId = userId,
            Name = request.Name,
            Type = request.Type
        };

        await _context.Categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(category.Adapt<CategoryResponse>());

    }
}
