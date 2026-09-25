using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Infrastructure.Persistence.EntitiesConfiguration;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Restrict: avoids a second cascade path User -> Budget -> Notification
        // on top of the direct User -> Notification cascade above (SQL Server forbids
        // multiple cascade paths to the same table).
        builder.HasOne(x => x.Budget)
            .WithMany(b => b.Notifications)
            .HasForeignKey(x => x.BudgetId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.UserId, x.IsRead });
    }
}
