namespace ExpenseTracker.Domain.Entities;

public class Budget
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public decimal Amount { get; set; }
    public int Month { get; set; } // 1-12
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Notification> Notifications { get; set; } = [];
}
