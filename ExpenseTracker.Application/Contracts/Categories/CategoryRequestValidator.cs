using FluentValidation;

namespace ExpenseTracker.Application.Contracts.Categories;

public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
{
    public CategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(2, 100);

        RuleFor(x => x.Type)
            .IsInEnum();
    }
}