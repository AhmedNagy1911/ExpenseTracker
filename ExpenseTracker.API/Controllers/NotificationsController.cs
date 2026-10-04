using ExpenseTracker.API.Extensions;
using ExpenseTracker.Application.Contracts.Notifications;
using ExpenseTracker.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class NotificationsController(INotificationService notificationService) : ControllerBase
{
    private readonly INotificationService _notificationService = notificationService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] NotificationFilters filters, CancellationToken cancellationToken)
    {
        var notifications = await _notificationService.GetAllAsync(UserId, filters, cancellationToken);
        return Ok(notifications);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken)
    {
        var count = await _notificationService.GetUnreadCountAsync(UserId, cancellationToken);
        return Ok(new { count });
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _notificationService.MarkAsReadAsync(UserId, id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var result = await _notificationService.MarkAllAsReadAsync(UserId, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}
