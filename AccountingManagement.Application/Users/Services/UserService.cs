using AccountingManagement.Application.Abstractions;
using AccountingManagement.Application.Users.DTOs;
using AccountingManagement.Domain.Entities;
using AccountingManagement.Domain.Interfaces;
using AccountingManagement.Domain.Exceptions;

namespace AccountingManagement.Application.Users.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UserService(
        IUserRepository userRepository, 
        IPasswordHasher passwordHasher,
        IDateTimeProvider dateTimeProvider
        )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _dateTimeProvider = dateTimeProvider;
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedDate = user.CreatedDate,
            LastLoginDate = user.LastLoginDate
        };
    }
    private List<UserDto> MapToDtoList(List<User> users) => users.Select(MapToDto).ToList();


    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        if (await _userRepository.UsernameExistsAsync(createUserDto.Username))
        {
            throw new ArgumentException("Username already exists.", nameof(createUserDto.Username));
        }

        var passwordHash = _passwordHasher.HashPassword(createUserDto.Password);
        var user = new User(
            createUserDto.Username,
            passwordHash,
            createUserDto.FullName,
            createUserDto.Email,
            createUserDto.Role)
        {
            CreatedDate = _dateTimeProvider.UtcNow
        };

        var newUser = await _userRepository.AddAsync(user);
        return MapToDto(newUser);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) throw new EntityNotFoundException(nameof(User), id);

        if (user.Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Cannot delete the main admin user.");
        }
        
        await _userRepository.DeleteAsync(id);
    }

    public async Task<List<UserDto>> GetAccountantsAsync()
    {
        var accountants = await _userRepository.GetAccountantsAsync();
        return MapToDtoList(accountants);
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return MapToDtoList(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToDto(user);
    }
    
    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return user == null ? null : MapToDto(user);
    }

    public async Task UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(updateUserDto.Id);
        if (user == null) throw new EntityNotFoundException(nameof(User), updateUserDto.Id);

        if (user.Username != updateUserDto.Username && await _userRepository.UsernameExistsAsync(updateUserDto.Username, user.Id))
        {
            throw new ArgumentException("New username already exists.", nameof(updateUserDto.Username));
        }
        
        if (user.Username.Equals("admin", StringComparison.OrdinalIgnoreCase) && 
            (updateUserDto.Role != Domain.Enums.UserRole.Admin || !updateUserDto.IsActive))
        {
            updateUserDto.Role = Domain.Enums.UserRole.Admin;
            updateUserDto.IsActive = true;
        }

        user.FullName = updateUserDto.FullName;
        user.Email = updateUserDto.Email;
        user.Role = updateUserDto.Role;
        if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
        {
            user.UpdatePassword(_passwordHasher.HashPassword(updateUserDto.Password));
        }

        if (updateUserDto.IsActive) user.Activate(); else user.Deactivate();
        
        user.LastModifiedDate = _dateTimeProvider.UtcNow;

        await _userRepository.UpdateAsync(user);
    }
    
    public async Task UpdateUserStatusAsync(int userId, bool isActive)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new EntityNotFoundException(nameof(User), userId);

        if (isActive) user.Activate(); else user.Deactivate();
        user.LastModifiedDate = _dateTimeProvider.UtcNow;
        await _userRepository.UpdateAsync(user);
    }
    
    public async Task<bool> UsernameExistsAsync(string username, int? currentUserId = null)
    {
        return await _userRepository.UsernameExistsAsync(username, currentUserId);
    }
}