using AccountingManagement.Domain.Entities;
using AccountingManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountingManagement.Infrastructure.Persistence.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _context;

    public CompanyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByIdAsync(int id)
    {
        return await _context.Companies.FindAsync(id);
    }

    public async Task<List<Company>> GetAllAsync()
    {
        return await _context.Companies.AsNoTracking().ToListAsync();
    }
    
    public async Task<List<Company>> GetByAccountantIdAsync(int accountantId)
    {
        return await _context.Companies
            .AsNoTracking()
            .Where(c => c.AccountantId == accountantId)
            .ToListAsync();
    }

    public async Task<Company> AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        return company;
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var company = await GetByIdAsync(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
    }
}