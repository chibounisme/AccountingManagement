using System.ComponentModel.DataAnnotations;

namespace AccountingManagement.Application.Users.DTOs;

public class LoginRequestDto
{
    [Required(ErrorMessage = "UsernameRequired")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "PasswordRequired")]
    public string Password { get; set; } = string.Empty;
}