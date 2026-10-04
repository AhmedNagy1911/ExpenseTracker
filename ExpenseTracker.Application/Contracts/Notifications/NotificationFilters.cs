namespace ExpenseTracker.Application.Contracts.Notifications;

public record NotificationFilters
{
    public bool? IsRead { get; init; }
}
