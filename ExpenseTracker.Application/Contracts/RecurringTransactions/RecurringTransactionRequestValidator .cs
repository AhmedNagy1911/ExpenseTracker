using FluentValidation;

namespace ExpenseTracker.Application.Contracts.RecurringTransactions;

public class RecurringTransactionRequestValidator : AbstractValidator<RecurringTransactionRequest>
{
    public RecurringTransactionRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.StartDate).NotEmpty();
    }
}
