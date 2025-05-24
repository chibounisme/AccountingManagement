using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingManagement.Models
{
    [Table("Companies")]
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string LegalName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        // For Unique or Indexed, configure with Fluent API in DbContext
        public string TaxIdentificationNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public string TradeRegisterNumber { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [MaxLength(20)]
        public string PostalCode { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Country { get; set; } = "Tunisie";

        [MaxLength(50)]
        [Phone] // Data annotation for validation
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        public string ActivityCode { get; set; } = string.Empty;

        public LegalForm LegalForm { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastModifiedDate { get; set; }

        // Foreign key property
        public int? AccountantId { get; set; }

        // Navigation property to the User (Accountant)
        // [ForeignKey("AccountantId")]
        // public virtual User? Accountant { get; set; } // Uncomment if you want navigation property
    }
}