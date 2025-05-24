using AccountingManagement.Data;
using AccountingManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingManagement.Services
{
    public class CompanyService
    {
        private readonly AppDbContext _dbContext;

        public CompanyService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Company>> GetCompaniesAsync(User currentUser)
        {
            IQueryable<Company> query = _dbContext.Companies.AsNoTracking();

            if (currentUser.Role == UserRole.Accountant)
            {
                query = query.Where(c => c.AccountantId == currentUser.Id);
            }
            // Admin sees all, so no filter needed beyond base query

            return await query.ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _dbContext.Companies.FindAsync(id);
        }

        public async Task<int> SaveCompanyAsync(Company company)
        {
            if (company.Id != 0) // Existing
            {
                company.LastModifiedDate = DateTime.UtcNow;
                _dbContext.Companies.Update(company);
            }
            else // New
            {
                company.CreationDate = DateTime.UtcNow;
                company.LastModifiedDate = DateTime.UtcNow; // Also set on creation
                _dbContext.Companies.Add(company);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _dbContext.Companies.FindAsync(id);
            if (company != null)
            {
                _dbContext.Companies.Remove(company);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}