using FluentValidation;

namespace ExpenseTracker.Application.Contracts.Reports;

public class MonthComparisonFiltersValidator : AbstractValidator<MonthComparisonFilters>
{
    public MonthComparisonFiltersValidator()
    {
        RuleFor(x => x.FromMonth).NotEmpty();
        RuleFor(x => x.ToMonth).NotEmpty();

        RuleFor(x => x.ToMonth)
            .GreaterThanOrEqualTo(x => x.FromMonth)
            .WithMessage("ToMonth must be after or equal to FromMonth");

        RuleFor(x => x)
            .Must(x => MonthsBetween(x.FromMonth, x.ToMonth) <= 36)
            .WithMessage("Date range cannot exceed 36 months");
    }

    private static int MonthsBetween(DateOnly from, DateOnly to) =>
        (to.Year - from.Year) * 12 + (to.Month - from.Month) + 1;
}