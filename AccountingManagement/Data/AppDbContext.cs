using AccountingManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography; // For password hashing helper
using System.Text;                  // For password hashing helper

namespace AccountingManagement.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Constructor remains the same.
            // Database creation/migration is handled externally (e.g., in MauiProgram.cs).
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique(); // Ensure Username is unique
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(250);
                entity.Property(e => e.FullName).HasMaxLength(150);
                entity.Property(e => e.Email).HasMaxLength(150);
            });

            // Configure Company entity
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(e => e.Id);
                // You can make TaxIdentificationNumber unique if required by Tunisian law for your internal system
                entity.HasIndex(e => e.TaxIdentificationNumber).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.LegalName).HasMaxLength(200);
                entity.Property(e => e.TaxIdentificationNumber).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(250);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.PostalCode).HasMaxLength(20);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(150);
                entity.Property(e => e.ActivityCode).HasMaxLength(50);

                // If you were to add navigation properties for relationships:
                // entity.HasOne(d => d.Accountant) // Assuming Company has a navigation property 'Accountant' of type User
                //    .WithMany(p => p.ManagedCompanies) // Assuming User has a collection 'ManagedCompanies'
                //    .HasForeignKey(d => d.AccountantId)
                //    .OnDelete(DeleteBehavior.SetNull); // Example: if accountant is deleted, set company's AccountantId to null
            });

            // Seed initial data (this data will be added when Database.EnsureCreated() is called)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Admin User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1, // Explicit ID for seeding with EF Core HasData
                    Username = "admin",
                    PasswordHash = HashPassword("admin123"), // Use your hashing method
                    FullName = "Administrateur Système",
                    Email = "admin@system.local",
                    Role = UserRole.Admin,
                    IsActive = true,
                    CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) // Use a fixed date for seeded data
                }
            );

            // Example of seeding a company (optional)
            // modelBuilder.Entity<Company>().HasData(
            //     new Company
            //     {
            //         Id = 1,
            //         Name = "Société Exemple SARL",
            //         LegalName = "Société Exemple SARL",
            //         TaxIdentificationNumber = "1234567A",
            //         TradeRegisterNumber = "B01987654",
            //         Address = "12 Rue de la Liberté",
            //         City = "Tunis",
            //         PostalCode = "1002",
            //         Country = "Tunisie",
            //         PhoneNumber = "71000000",
            //         Email = "contact@exemple.tn",
            //         ActivityCode = "6201Z",
            //         LegalForm = LegalForm.SARL,
            //         CreationDate = new DateTime(2023, 5, 10, 0,0,0, DateTimeKind.Utc),
            //         // AccountantId = 1 // If you want to link it to the admin or another seeded accountant
            //     }
            // );
        }

        // Password hashing methods (consider moving to a dedicated service/utility class)
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return HashPassword(password) == hashedPassword;
        }
    }
}