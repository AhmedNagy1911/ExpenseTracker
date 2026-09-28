using FluentValidation;

namespace ExpenseTracker.Application.Contracts.Transactions;

public class TransactionRequestValidator : AbstractValidator<TransactionRequest>
{
    public TransactionRequestValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
