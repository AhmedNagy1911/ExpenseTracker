using FluentValidation;

namespace ExpenseTracker.Application.Contracts.Transactions;

public class TransactionFiltersValidator : AbstractValidator<TransactionFilters>
{
    public TransactionFiltersValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);   // سقف عشان محدش يطلب 1,000,000 صف
        RuleFor(x => x.SortDirection)
            .Must(x => x is null || x.Equals("ASC", StringComparison.OrdinalIgnoreCase) || x.Equals("DESC", StringComparison.OrdinalIgnoreCase))
            .WithMessage("SortDirection must be ASC or DESC");
        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
    }
}