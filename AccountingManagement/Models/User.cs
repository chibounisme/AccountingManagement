using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // For [Table]

namespace AccountingManagement.Models
{
    [Table("Users")] // EF Core respects this
    public class User
    {
        [Key] // Explicitly mark as Key, though EF Core would infer it
        public int Id { get; set; }

        [Required] // Good practice for EF Core
        [MaxLength(100)]
        // For Unique constraint, it's better to configure this using Fluent API in DbContext
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(150)]
        [EmailAddress] // Data annotation for validation
        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginDate { get; set; }

        // Navigation Property (if you want to link Companies back to User)
        // public virtual ICollection<Company> ManagedCompanies { get; set; } = new List<Company>();
    }
}