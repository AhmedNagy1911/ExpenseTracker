using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public Guid BudgetId { get; set; }
    public Budget Budget { get; set; } = null!;
    public NotificationType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
}