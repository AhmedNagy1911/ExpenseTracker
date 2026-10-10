using ExpenseTracker.Application.Contracts.Notifications;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ExpenseTracker.API.Hubs;

public class SignalRNotificationPublisher(IHubContext<NotificationHub> hubContext) : INotificationPublisher
{
    private readonly IHubContext<NotificationHub> _hubContext = hubContext;

    public Task PublishAsync(string userId, NotificationResponse notification, CancellationToken cancellationToken = default) =>
        _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notification, cancellationToken);
}