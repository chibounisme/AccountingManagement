using AccountingManagement.Domain.Entities;

namespace AccountingManagement.Domain.Interfaces;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(int id);
    Task<List<Company>> GetAllAsync();
    Task<List<Company>> GetByAccountantIdAsync(int accountantId);
    Task<Company> AddAsync(Company company);
    Task UpdateAsync(Company company);
    Task DeleteAsync(int id);
}