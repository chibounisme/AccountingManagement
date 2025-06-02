using AccountingManagement.Application.Abstractions;
using AccountingManagement.Domain.Entities;
using AccountingManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AccountingManagement.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    private readonly IPasswordHasher _passwordHasher; // Injected for seeding

    public DbSet<User> Users => Set<User>();
    public DbSet<Company> Companies => Set<Company>();

    public AppDbContext(DbContextOptions<AppDbContext> options, IPasswordHasher passwordHasher) : base(options)
    {
        _passwordHasher = passwordHasher;
    }
    
    // Parameterless constructor for design-time tools if password hasher is an issue
    // public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User("admin", _passwordHasher.HashPassword("admin123"), "System Administrator", "admin@system.local", UserRole.Admin)
            {
                Id = 1,
                CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            }
        );
    }
}