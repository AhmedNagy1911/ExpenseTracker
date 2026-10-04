using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Contracts.Notifications;

public record NotificationResponse(
    Guid Id,
    Guid BudgetId,
    string CategoryName,   // Mapster: Budget.Category.Name
    NotificationType Type,
    string Message,
    DateTime SentAt,
    bool IsRead
);