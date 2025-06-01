using AccountingManagement.Domain.Entities;

namespace AccountingManagement.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<List<User>> GetAllAsync();
    Task<List<User>> GetAccountantsAsync();
    Task<User> AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
    Task<bool> UsernameExistsAsync(string username, int? currentUserId = null);
}