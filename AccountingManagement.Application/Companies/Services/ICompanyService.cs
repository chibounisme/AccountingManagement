using AccountingManagement.Application.Companies.DTOs;

namespace AccountingManagement.Application.Companies.Services;

public interface ICompanyService
{
    Task<CompanyDto?> GetCompanyByIdAsync(int id);
    Task<List<CompanyDto>> GetAllCompaniesAsync();
    Task<List<CompanyDto>> GetCompaniesForCurrentUserAsync();
    Task<CompanyDto> CreateCompanyAsync(CreateCompanyDto createCompanyDto);
    Task UpdateCompanyAsync(UpdateCompanyDto updateCompanyDto);
    Task DeleteCompanyAsync(int id);
}