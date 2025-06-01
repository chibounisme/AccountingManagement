using AccountingManagement.Domain.Enums;

namespace AccountingManagement.Application.Abstractions;

public interface ICurrentUserService
{
    int? UserId { get; }
    string? Username { get; }
    UserRole? UserRole { get; }
    bool IsAuthenticated { get; }
}