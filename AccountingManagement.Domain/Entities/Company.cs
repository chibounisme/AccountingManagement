using AccountingManagement.Domain.Enums;
using AccountingManagement.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AccountingManagement.Domain.Entities;

public class Company : AuditableEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string LegalName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string TaxIdentificationNumber { get; set; } = string.Empty;

    [MaxLength(50)]
    public string TradeRegisterNumber { get; set; } = string.Empty;

    [MaxLength(250)]
    public string Address { get; set; } = string.Empty; // Could be a Value Object

    [MaxLength(100)]
    public string City { get; set; } = string.Empty;

    [MaxLength(20)]
    public string PostalCode { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Country { get; set; } = "Tunisie";

    [MaxLength(50)]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string ActivityCode { get; set; } = string.Empty;

    public LegalForm LegalForm { get; set; }
    
    public DateTime CompanyCreationDate { get; set; }

    public int? AccountantId { get; private set; }
    // Navigation property for EF Core, if direct navigation is desired.
    // Typically, repositories abstract this away.
    // public User? Accountant { get; private set; }

    // Private constructor for EF Core
    private Company() { }

    public Company(string name, string taxId, LegalForm legalForm, DateTime companyCreationDate)
    {
        Name = name;
        TaxIdentificationNumber = taxId;
        LegalForm = legalForm;
        CompanyCreationDate = companyCreationDate;
        CreatedDate = DateTime.UtcNow;
    }
    
    public void AssignAccountant(int accountantId)
    {
        // Potentially add domain rules, e.g., check if accountant exists (though this is more an app service concern)
        AccountantId = accountantId;
        LastModifiedDate = DateTime.UtcNow;
    }

    public void UnassignAccountant()
    {
        AccountantId = null;
        LastModifiedDate = DateTime.UtcNow;
    }
}