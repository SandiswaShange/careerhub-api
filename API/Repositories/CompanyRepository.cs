using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;

public class CompanyRepository(JobListingDbContext db)
    : ICompanyRepository
{
    private readonly JobListingDbContext _db = db;

    public async Task<bool> CompanyExistsAsync(Guid companyId)
    {
        return await _db.Companies
            .AnyAsync(c => c.Id == companyId);
    }

    public async Task<Company?> GetByIdAsync(Guid companyId)
    {
        return await _db.Companies
            .FirstOrDefaultAsync(c => c.Id == companyId);
    }

    public async Task<Company?> GetByNameAsync(string companyName)
    {
        return await _db.Companies
            .FirstOrDefaultAsync(c => c.Name == companyName);
    }
}