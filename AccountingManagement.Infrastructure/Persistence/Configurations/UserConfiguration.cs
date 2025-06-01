using AccountingManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingManagement.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(100);
        builder.HasIndex(u => u.Username).IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(250); // Adjust if hasher produces longer/shorter

        builder.Property(u => u.FullName)
            .HasMaxLength(150);

        builder.Property(u => u.Email)
            .HasMaxLength(150);

        builder.Property(u => u.Role)
            .HasConversion<string>() // Store enum as string
            .HasMaxLength(50);

        builder.Property(u => u.IsActive);
        builder.Property(u => u.CreatedDate);
        builder.Property(u => u.LastLoginDate);
        builder.Property(u => u.CreatedBy).HasMaxLength(100);
        builder.Property(u => u.LastModifiedDate);
        builder.Property(u => u.LastModifiedBy).HasMaxLength(100);
    }
}