using AccountingManagement.Application.Users.DTOs;
using AccountingManagement.Domain.Entities;

namespace AccountingManagement.Application.Users.Services;

public interface IAuthService
{
    Task<UserDto?> LoginAsync(string username, string password);
}