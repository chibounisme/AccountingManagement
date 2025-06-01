using AccountingManagement.Domain.Entities;
using AccountingManagement.Domain.Enums;
using AccountingManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountingManagement.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().ToListAsync();
    }
    
    public async Task<List<User>> GetAccountantsAsync()
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Role == UserRole.Accountant)
            .ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<bool> UsernameExistsAsync(string username, int? currentUserId = null)
    {
        if (currentUserId.HasValue)
        {
            return await _context.Users.AnyAsync(u => u.Username == username && u.Id != currentUserId.Value);
        }
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
}