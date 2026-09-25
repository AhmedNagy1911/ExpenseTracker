using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Entities;

public class Category
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Transaction> Transactions { get; set; } = [];
    public ICollection<RecurringTransaction> RecurringTransactions { get; set; } = [];
    public ICollection<Budget> Budgets { get; set; } = [];
}
