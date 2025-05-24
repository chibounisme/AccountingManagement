using AccountingManagement.Data;
using AccountingManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingManagement.Services
{
    public class UserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _dbContext.Users.AsNoTracking().ToListAsync();
        }

        public async Task<List<User>> GetAccountantsAsync()
        {
            return await _dbContext.Users
                                   .AsNoTracking()
                                   .Where(u => u.Role == UserRole.Accountant)
                                   .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            // FindAsync is good for PK lookups and tracks by default if not AsNoTracking
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> UsernameExistsAsync(string username, int? currentUserId = null)
        {
            if (currentUserId.HasValue)
            {
                return await _dbContext.Users.AnyAsync(u => u.Username == username && u.Id != currentUserId.Value);
            }
            return await _dbContext.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<int> SaveUserAsync(User user, string? password = null)
        {
            if (!string.IsNullOrWhiteSpace(password))
            {
                user.PasswordHash = AppDbContext.HashPassword(password);
            }

            if (user.Id != 0) // Existing user
            {
                _dbContext.Users.Update(user);
            }
            else // New user
            {
                user.CreatedDate = DateTime.UtcNow;
                _dbContext.Users.Add(user);
            }
            return await _dbContext.SaveChangesAsync(); // Returns number of state entries written to the database
        }

        public async Task<bool> UpdateUserStatusAsync(int userId, bool isActive)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user != null)
            {
                user.IsActive = isActive;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                _dbContext.Users.Remove(user);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}