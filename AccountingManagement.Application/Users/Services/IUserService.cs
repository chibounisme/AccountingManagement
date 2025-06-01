using AccountingManagement.Application.Users.DTOs;
using AccountingManagement.Domain.Entities;

namespace AccountingManagement.Application.Users.Services;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<List<UserDto>> GetAccountantsAsync();
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task UpdateUserAsync(UpdateUserDto updateUserDto);
    Task DeleteUserAsync(int id);
    Task UpdateUserStatusAsync(int userId, bool isActive);
    Task<bool> UsernameExistsAsync(string username, int? currentUserId = null);
}