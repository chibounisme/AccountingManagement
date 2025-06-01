using AccountingManagement.Domain.Enums;

namespace AccountingManagement.Application.Companies.DTOs;

public class CompanyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string TaxIdentificationNumber { get; set; } = string.Empty;
    public string TradeRegisterNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ActivityCode { get; set; } = string.Empty;
    public LegalForm LegalForm { get; set; }
    public DateTime CompanyCreationDate { get; set; }
    public int? AccountantId { get; set; }
    public string? AccountantFullName { get; set; } // For display
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}