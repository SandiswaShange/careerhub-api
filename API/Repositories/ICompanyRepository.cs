using API.Models;
public  interface ICompanyRepository
{
    Task<bool> CompanyExistsAsync(Guid companyId);
    Task<Company?> GetByIdAsync(Guid companyId);
    Task<Company?> GetByNameAsync(string companyName);
}