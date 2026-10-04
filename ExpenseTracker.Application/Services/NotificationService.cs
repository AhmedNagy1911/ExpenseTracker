using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Contracts.Notifications;
using ExpenseTracker.Application.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Application.Services;

public class NotificationService(IApplicationDbContext context) : INotificationService
{
    private readonly IApplicationDbContext _context = context;

    public async Task<IEnumerable<NotificationResponse>> GetAllAsync(string userId, NotificationFilters filters, CancellationToken cancellationToken = default)
    {
        var query = _context.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == userId);

        if (filters.IsRead.HasValue)
            query = query.Where(x => x.IsRead == filters.IsRead);

        return await query
            .OrderByDescending(x => x.SentAt)
            .ProjectToType<NotificationResponse>()
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default) =>
        await _context.Notifications
            .AsNoTracking()
            .CountAsync(x => x.UserId == userId && !x.IsRead, cancellationToken);


}