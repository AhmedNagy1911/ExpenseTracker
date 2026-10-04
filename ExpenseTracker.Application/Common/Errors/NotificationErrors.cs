using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.Common.Errors;

public record NotificationErrors
{
    public static readonly Error NotificationNotFound =
        new("Notification.NotificationNotFound", "Notification is not found", StatusCodes.Status404NotFound);
}
