using AccountingManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AccountingManagement.Application.Companies.DTOs;

public class CreateCompanyDto
{
    [Required(ErrorMessage = "CompanyNameRequired")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string LegalName { get; set; } = string.Empty;

    [Required(ErrorMessage = "TaxIdRequired")]
    [MaxLength(50)]
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
    [Phone(ErrorMessage = "InvalidPhoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [MaxLength(150)]
    [EmailAddress(ErrorMessage = "InvalidEmailFormat")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string ActivityCode { get; set; } = string.Empty;

    public LegalForm LegalForm { get; set; }
    
    [Required(ErrorMessage = "CreationDateRequired")]
    public DateTime CompanyCreationDate { get; set; } = DateTime.Today;
    
    public int? AccountantId { get; set; }
}