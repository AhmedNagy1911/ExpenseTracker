using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure.Helpers;
using ExpenseTracker.Infrastructure.Persistence;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Infrastructure.Jobs;

public class BudgetCheckJob(ApplicationDbContext context, ILogger<BudgetCheckJob> logger) : IBudgetCheckJob
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<BudgetCheckJob> _logger = logger;

    private const decimal ApproachingThreshold = 0.8m; // 80%

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var month = today.Month;
        var year = today.Year;

        var budgets = await _context.Budgets
            .Where(b => b.Month == month && b.Year == year)
            .Join(_context.Users.Where(u => !u.IsDisabled),
                  b => b.UserId, u => u.Id,
                  (b, u) => new { Budget = b, User = u })
            .ToListAsync(cancellationToken);

        if (budgets.Count == 0)
        {
            _logger.LogInformation("No budgets to check for {Month}/{Year}.", month, year);
            return;
        }

        var categoryNames = await _context.Categories
            .Where(c => budgets.Select(x => x.Budget.CategoryId).Contains(c.Id))
            .ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken);

        var budgetIds = budgets.Select(x => x.Budget.Id).ToList();

        var periodStart = new DateTime(year, month, 1);
        var periodEnd = periodStart.AddMonths(1);

        var spentByCategory = await _context.Transactions
            .Where(t => t.Type == TransactionType.Expense && t.Date >= periodStart && t.Date < periodEnd)
            .GroupBy(t => new { t.UserId, t.CategoryId })
            .Select(g => new { g.Key.UserId, g.Key.CategoryId, Total = g.Sum(x => x.Amount) })
            .ToListAsync(cancellationToken);

        var alreadySentSet = (await _context.Notifications
            .Where(n => budgetIds.Contains(n.BudgetId))
            .Select(n => new { n.BudgetId, n.Type })
            .ToListAsync(cancellationToken))
            .Select(x => (x.BudgetId, x.Type))
            .ToHashSet();

        // بنحتفظ بالـ context (User + spent + category) لكل Notification جديدة عشان نستخدمها بعدين في الإيميل
        // من غير ما نعمل query تاني أو نعتمد على Order
        var toSend = new List<(Notification Notification, ApplicationUser User, string CategoryName, decimal Spent, decimal BudgetAmount)>();

        foreach (var entry in budgets)
        {
            var budget = entry.Budget;

            if (budget.Amount <= 0)
                continue;

            var spent = spentByCategory
                .SingleOrDefault(x => x.UserId == budget.UserId && x.CategoryId == budget.CategoryId)
                ?.Total ?? 0;

            var ratio = spent / budget.Amount;

            NotificationType? type = ratio switch
            {
                > 1m => NotificationType.Exceeded,
                >= ApproachingThreshold => NotificationType.Approaching,
                _ => null
            };

            if (type is null || alreadySentSet.Contains((budget.Id, type.Value)))
                continue;

            var categoryName = categoryNames.GetValueOrDefault(budget.CategoryId, "Unknown");

            var message = type == NotificationType.Exceeded
                ? $"You exceeded your '{categoryName}' budget by {spent - budget.Amount:0.##} EGP this month."
                : $"You have used {ratio:P0} of your '{categoryName}' budget this month.";

            var notification = new Notification
            {
                UserId = budget.UserId,
                BudgetId = budget.Id,
                Type = type.Value,
                Message = message
            };

            toSend.Add((notification, entry.User, categoryName, spent, budget.Amount));
        }

        if (toSend.Count == 0)
        {
            _logger.LogInformation("No new budget alerts to send.");
            return;
        }

        await _context.Notifications.AddRangeAsync(toSend.Select(x => x.Notification), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // الإيميلات بتتبعت Async عن طريق Hangfire بعد ما الـ Notifications اتحفظت في الـ DB بنجاح.
        // لو إيميل واحد فشل (SMTP down مثلاً)، Hangfire هيعمل Retry تلقائي ليه لوحده من غير ما يأثر على الباقي،
        // ومن غير ما يوقف باقي الـ Job أو يعمل Rollback للـ Notifications المحفوظة.
        foreach (var item in toSend)
        {
            if (string.IsNullOrWhiteSpace(item.User.Email))
                continue;

            var isExceeded = item.Notification.Type == NotificationType.Exceeded;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("BudgetAlert",
                templateModel: new Dictionary<string, string>
                {
                    { "{{headerColor}}", isExceeded ? "#C0392B" : "#A8791A" },
                    { "{{title}}", isExceeded ? "Budget Exceeded" : "Approaching Your Budget Limit" },
                    { "{{name}}",$"{item.User.FirstName} {item.User.LastName}" },
                    { "{{message}}", item.Notification.Message },
                    { "{{categoryName}}", item.CategoryName },
                    { "{{budgetAmount}}", item.BudgetAmount.ToString("0.##") },
                    { "{{spentAmount}}", item.Spent.ToString("0.##") },
                    { "{{appUrl}}", "https://your-frontend-domain.com/budgets" } // غيّرها لدومين الفرونت عندك
                });

            var subject = isExceeded
                ? "⚠️ ExpenseTracker: Budget Exceeded"
                : "⚠️ ExpenseTracker: Approaching Budget Limit";

            BackgroundJob.Enqueue<IEmailSender>(sender => sender.SendEmailAsync(item.User.Email!, subject, emailBody));
        }

        _logger.LogInformation("Generated {Count} budget alerts and enqueued their emails.", toSend.Count);
    }
}