using AccountingManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountingManagement.Application.Abstractions;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Company> Companies { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}