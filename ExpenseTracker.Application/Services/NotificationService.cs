using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Common.Errors;
using ExpenseTracker.Application.Common.Results;
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


    public async Task<Result> MarkAsReadAsync(string userId, Guid id, CancellationToken cancellationToken = default)
    {
        var notification = await _context.Notifications
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Id == id, cancellationToken);

        if (notification is null)
            return Result.Failure(NotificationErrors.NotificationNotFound);

        notification.IsRead = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default)
    {
        await _context.Notifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.IsRead, true), cancellationToken);

        return Result.Success();
    }

}