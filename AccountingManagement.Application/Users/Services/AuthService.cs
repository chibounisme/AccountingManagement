using AccountingManagement.Application.Abstractions;
using AccountingManagement.Application.Users.DTOs;
using AccountingManagement.Domain.Entities;
using AccountingManagement.Domain.Interfaces;
using AccountingManagement.Domain.Exceptions;
// using AutoMapper;

namespace AccountingManagement.Application.Users.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider;
    // private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IDateTimeProvider dateTimeProvider
        // IMapper mapper
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _dateTimeProvider = dateTimeProvider;
        // _mapper = mapper;
    }

    // Manual mapping for now
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

    public async Task<UserDto?> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user == null || !user.IsActive || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
        {
            return null; // Or throw specific InvalidCredentialsException
        }

        user.LastLoginDate = _dateTimeProvider.UtcNow;
        await _userRepository.UpdateAsync(user); // Update last login date

        return MapToDto(user);
    }
}