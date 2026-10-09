using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Common.Errors;

public record CategoryErrors
{
    public static readonly Error CategoryNotFound =
        new("Category.CategoryNotFound", "Category is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedCategory =
        new("Category.DuplicatedCategory", "You already have a category with the same name and type", StatusCodes.Status409Conflict);

    public static readonly Error CategoryInUse =
        new("Category.CategoryInUse", "Cannot delete this category because it has related transactions, recurring transactions or budgets", StatusCodes.Status409Conflict);

    public static readonly Error CategoryTypeLocked =
        new("Category.CategoryTypeLocked", "Cannot change the type of a category that has related transactions, recurring transactions or budgets", StatusCodes.Status409Conflict);
}