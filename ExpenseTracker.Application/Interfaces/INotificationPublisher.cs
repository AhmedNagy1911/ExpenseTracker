using ExpenseTracker.Application.Contracts.Notifications;

namespace ExpenseTracker.Application.Interfaces;

public interface INotificationPublisher
{
    Task PublishAsync(string userId, NotificationResponse notification, CancellationToken cancellationToken = default);
}