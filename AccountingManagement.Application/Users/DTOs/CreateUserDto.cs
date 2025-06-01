using AccountingManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccountingManagement.Application.Users.DTOs;

public class CreateUserDto
{
    [Required(ErrorMessage = "UsernameRequired")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "UsernameLength")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "PasswordRequired")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "PasswordLength")]
    public string Password { get; set; } = string.Empty;
    
    [Compare(nameof(Password), ErrorMessage = "PasswordsDontMatch")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "FullNameRequired")]
    [StringLength(150, ErrorMessage = "FullNameLength")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "EmailRequired")]
    [EmailAddress(ErrorMessage = "InvalidEmailFormat")]
    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Accountant;
}