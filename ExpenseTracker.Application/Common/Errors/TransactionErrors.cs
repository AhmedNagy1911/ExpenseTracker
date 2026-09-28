using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Common.Errors;

public record TransactionErrors
{
    public static readonly Error TransactionNotFound =
        new("Transaction.TransactionNotFound", "Transaction is not found", StatusCodes.Status404NotFound);
}