using ExpenseTracker.Application.Common.Results;
using ExpenseTracker.Application.Contracts.Notifications;

namespace ExpenseTracker.Application.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<NotificationResponse>> GetAllAsync(string userId, NotificationFilters filters, CancellationToken cancellationToken = default);
    Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default);
    Task<Result> MarkAsReadAsync(string userId, Guid id, CancellationToken cancellationToken = default);
    Task<Result> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default);
}
