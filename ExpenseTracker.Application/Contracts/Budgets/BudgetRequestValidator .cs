using FluentValidation;

namespace ExpenseTracker.Application.Contracts.Budgets;


public class BudgetRequestValidator : AbstractValidator<BudgetRequest>
{
    public BudgetRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();

        RuleFor(x => x.Amount).GreaterThan(0);

        RuleFor(x => x.Month).InclusiveBetween(1, 12);

        RuleFor(x => x.Year).InclusiveBetween(2020, 2100);
    }
}