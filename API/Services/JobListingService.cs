using API.DTOs;
using API.Exceptions;
using API.Models;

public class JobListingService(IJobListingRepository jobRepository,ICompanyRepository companyRepository) : IJobListingService
{
    private readonly IJobListingRepository _jobRepository = jobRepository;

    private readonly ICompanyRepository _companyRepository = companyRepository;

    public async Task CloseListingAsync(Guid listingId)
    {
        var listing = await _jobRepository.GetByIdAsync(listingId);

        if (listing is null)
        {
            throw new JobNotFoundException(listingId);
        }

        if (!listing.IsActive)
        {
            await _jobRepository.CloseAsync(listingId);
        }
        else
        {
            throw new ListingClosedException(listingId);
        }
    }

    public async Task<JobResponse> CreateListingAsync(CreateJobRequest dto)
    {
        var company = await _companyRepository.GetByNameAsync(dto.Company);

    if (company is null)
    {
        throw new CompanyNotFoundException(
            dto.Company);
    }

    if (dto.ClosingDate <= DateTime.UtcNow)
    {
        throw new ArgumentException("Closing date must be in the future.");
    }

    var listing = new JobListing
    {
        Id = Guid.NewGuid(),
        Title = dto.Title,
        Description = dto.Description,
        CompanyId = company.Id,
        Company = company,
        Location = dto.Location,
        Type = dto.Type,
        MinSalary = dto.SalaryMin,
        MaxSalary = dto.SalaryMax,
        PostedAt = DateTime.UtcNow,
        ClosingDate = dto.ClosingDate,
        IsActive = true
    };

    await _jobRepository.AddAsync(listing);

    return new JobResponse(
        listing.Id,
        listing.Title,
        listing.Description,
        company.Name,
        listing.Location,
        listing.Type,
        listing.PostedAt,
        listing.IsActive,
        listing.MinSalary ?? 0,
        listing.MaxSalary ?? 0,
        GetSalaryDisplay(listing)
    );
    }
    public async Task<IEnumerable<JobListResponse>> GetActiveListingsAsync()
    {
         return await _jobRepository.GetActiveListingsAsync();
    }

    public async Task<JobDetailResponse> GetListingAsync(Guid listingId)
    {
        return await _jobRepository.GetListingDetailsAsync(listingId);
    }

    public async Task<JobResponse> UpdateListingAsync(Guid listingId, UpdateJobRequest dto)
    {
            var listing =
        await _jobRepository.GetByIdAsync(listingId);

    if (listing is null)
    {
        throw new JobNotFoundException(listingId);
    }

    if (!listing.IsActive)
    {
        throw new ListingClosedException(listingId);
    }

    var company =
        await _companyRepository.GetByNameAsync(dto.Company);

    if (company is null)
    {
        throw new CompanyNotFoundException(dto.Company);
    }

    listing.Title = dto.Title;
    listing.Description = dto.Description;
    listing.CompanyId = company.Id;
    listing.Company = company;
    listing.Location = dto.Location;
    listing.Type = dto.Type;
    listing.MinSalary = dto.SalaryMin;
    listing.MaxSalary = dto.SalaryMax;

    await _jobRepository.UpdateAsync(listing);

    return new JobResponse(
        listing.Id,
        listing.Title,
        listing.Description,
        company.Name,
        listing.Location,
        listing.Type,
        listing.PostedAt,
        listing.IsActive,
        listing.MinSalary ?? 0,
        listing.MaxSalary ?? 0,
        GetSalaryDisplay(listing)
    );
    }
//=====================================================================================================================================
    private static string GetSalaryDisplay(JobListing job)
{
    if (job.MinSalary.HasValue && job.MaxSalary.HasValue)
    {
        return $"R{job.MinSalary} - R{job.MaxSalary}/month";
    }

    if (job.MinSalary.HasValue)
    {
        return $"From R{job.MinSalary}/month";
    }

    return "Salary not specified";
}

    public async Task<PagedResponse<JobListResponse>> GetActiveListingsPagedAsync(Guid companyId, int page, int pageSize, JobListingFilterQuery filter)
    {
        return await _jobRepository.GetActiveListingsPagedAsync(companyId,page,pageSize, filter);
    }
}