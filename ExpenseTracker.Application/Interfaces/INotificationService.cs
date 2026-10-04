using ExpenseTracker.Application.Contracts.Notifications;

namespace ExpenseTracker.Application.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<NotificationResponse>> GetAllAsync(string userId, NotificationFilters filters, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default);
}
