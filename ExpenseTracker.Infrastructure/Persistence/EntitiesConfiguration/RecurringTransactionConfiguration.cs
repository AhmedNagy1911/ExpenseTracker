using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.EntitiesConfiguration;

public class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
{
    public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Amount)
            .HasPrecision(18, 2);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.Frequency)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict: deleting a Category that still has RecurringTransactions is blocked.
        builder.HasOne(x => x.Category)
            .WithMany(c => c.RecurringTransactions)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Picked up by the Hangfire job (WHERE IsActive AND NextRunDate <= today).
        builder.HasIndex(x => new { x.IsActive, x.NextRunDate });
    }
}
