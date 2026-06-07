public  interface ICompanyRepository
{
    Task<bool> CompanyExistsAsync(Guid companyId);
}