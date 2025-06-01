using AccountingManagement.Domain.Enums;
using AccountingManagement.Domain.Common;
using System.ComponentModel.DataAnnotations; // Keep for basic validation attributes if useful for domain

namespace AccountingManagement.Domain.Entities;

public class User : AuditableEntity
{
    [Required]
    [MaxLength(100)]
    public string Username { get; private set; } = string.Empty;

    [Required]
    public string PasswordHash { get; private set; } = string.Empty;

    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginDate { get; set; }

    // Private constructor for EF Core
    private User() { }

    public User(string username, string passwordHash, string fullName, string email, UserRole role)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(passwordHash));
        
        Username = username;
        PasswordHash = passwordHash;
        FullName = fullName;
        Email = email;
        Role = role;
        IsActive = true; // Default to active
        CreatedDate = DateTime.UtcNow;
    }

    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("New password hash cannot be empty.", nameof(newPasswordHash));
        PasswordHash = newPasswordHash;
        LastModifiedDate = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        LastModifiedDate = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        // Domain rule: Admin user cannot be deactivated.
        if (Role == UserRole.Admin && Username.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            // Consider throwing a custom DomainException
            throw new InvalidOperationException("The main admin user cannot be deactivated.");
        }
        IsActive = false;
        LastModifiedDate = DateTime.UtcNow;
    }

    public void UpdateProfile(string fullName, string email)
    {
        FullName = fullName;
        Email = email; // Consider email validation VO
        LastModifiedDate = DateTime.UtcNow;
    }
}