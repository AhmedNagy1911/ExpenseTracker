using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Entities;

public class RecurringTransaction
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string? Description { get; set; }
    public RecurringFrequency Frequency { get; set; } = RecurringFrequency.Monthly;
    public DateTime StartDate { get; set; }

    // Used by the Hangfire job to know when to generate the next Transaction.
    public DateTime NextRunDate { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Transaction> GeneratedTransactions { get; set; } = [];
}
