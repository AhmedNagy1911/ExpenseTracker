using FluentValidation;

namespace ExpenseTracker.Application.Contracts.RecurringTransactions;

public class RecurringTransactionFiltersValidator : AbstractValidator<RecurringTransactionFilters>
{
    public RecurringTransactionFiltersValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        RuleFor(x => x.SortDirection)
            .Must(x => x is null || x.Equals("ASC", StringComparison.OrdinalIgnoreCase) || x.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortDirection must be ASC or DESC");
    }
}