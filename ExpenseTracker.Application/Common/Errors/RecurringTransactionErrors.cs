using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Common.Errors;

public record RecurringTransactionErrors
{
    public static readonly Error NotFound =
        new("RecurringTransaction.NotFound", "Recurring transaction is not found", StatusCodes.Status404NotFound);
}