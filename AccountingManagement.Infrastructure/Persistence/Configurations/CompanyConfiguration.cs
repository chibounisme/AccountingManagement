using AccountingManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingManagement.Infrastructure.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.TaxIdentificationNumber)
            .IsRequired()
            .HasMaxLength(50);
        builder.HasIndex(c => c.TaxIdentificationNumber).IsUnique();
        
        builder.Property(c => c.LegalName).HasMaxLength(200);
        builder.Property(c => c.TradeRegisterNumber).HasMaxLength(50);
        builder.Property(c => c.Address).HasMaxLength(250);
        builder.Property(c => c.City).HasMaxLength(100);
        builder.Property(c => c.PostalCode).HasMaxLength(20);
        builder.Property(c => c.Country).HasMaxLength(100);
        builder.Property(c => c.PhoneNumber).HasMaxLength(50);
        builder.Property(c => c.Email).HasMaxLength(150);
        builder.Property(c => c.ActivityCode).HasMaxLength(50);

        builder.Property(c => c.LegalForm)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.CreatedDate);
        builder.Property(c => c.LastModifiedDate);
        builder.Property(c => c.CreatedBy).HasMaxLength(100);
        builder.Property(c => c.LastModifiedBy).HasMaxLength(100);


        // Relationship with User (Accountant)
        // If you had a navigation property User Accountant in Company:
        // builder.HasOne(c => c.Accountant)
        //    .WithMany() // Or WithMany(u => u.ManagedCompanies) if User has a collection
        //    .HasForeignKey(c => c.AccountantId)
        //    .OnDelete(DeleteBehavior.SetNull); // If accountant is deleted, set company's AccountantId to null
    }
}