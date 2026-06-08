using API.Models;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace API.Data;

public class JobListingDbContext(DbContextOptions<JobListingDbContext> options): DbContext(options)
{
    public DbSet<JobListing> JobListings => Set<JobListing>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Applicant> Applicants => Set<Applicant>();
    public DbSet<Application> Applications => Set<Application>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
//=======================================================JobListing entity===============================================================

        modelBuilder.Entity<JobListing>(entity =>
        {
        entity.ToTable("job_listings", t =>{
        t.HasCheckConstraint(
        "ck_job_listings_min_salary_positive",
        "\"MinSalary\" IS NULL OR \"MinSalary\" > 0");

        t.HasCheckConstraint(
        "ck_job_listings_salary_range",
        "\"MinSalary\" IS NULL OR \"MaxSalary\" IS NULL OR \"MaxSalary\" > \"MinSalary\"");

        t.HasCheckConstraint(
        "ck_job_listings_closing_after_posted",
        "\"ClosingDate\" > \"PostedAt\"");
        });
        entity.HasKey(j => j.Id);

        entity.Property(j => j.Id).ValueGeneratedNever();

        entity.Property(j => j.Title).IsRequired().HasMaxLength(100);

        entity.Property(j => j.CompanyId).IsRequired();

        entity.Property(j => j.Description).IsRequired().HasMaxLength(2000);

        entity.Property(j => j.Location).IsRequired().HasMaxLength(100);
        
        entity.Property(j => j.ClosingDate).IsRequired();

        //indexes
        entity.HasIndex(j => new { j.IsActive, j.ClosingDate }).HasDatabaseName("ix_job_listings_isactive_closingdate");
        entity.HasIndex(j => new { j.CompanyId, j.IsActive }).HasDatabaseName("ix_job_listings_companyid_isactive");
        
        //search vector
        entity.Property<NpgsqlTypes.NpgsqlTsVector>("SearchVector")
        .HasComputedColumnSql(
        @"to_tsvector(
            'english',
            coalesce(""Title"", '') || ' ' ||
            coalesce(""Description"", '')
        )",
        stored: true);

        //GIN index
        entity.HasIndex("SearchVector").HasMethod("GIN").HasDatabaseName("ix_job_listings_searchvector");
    });

//=======================================================Company entity===============================================================
        modelBuilder.Entity<Company>(entity =>
        {
    entity.ToTable("companies");

    entity.HasKey(c => c.Id);

    entity.Property(c => c.Id).ValueGeneratedNever();

    entity.Property(c => c.Name).IsRequired().HasMaxLength(100);

    entity.HasIndex(c => c.Name).IsUnique();
    });
    
//=======================================================Applicant entity===============================================================
        modelBuilder.Entity<Applicant>(entity =>
        {
    entity.ToTable("applicants");

    entity.HasKey(a => a.Id);

    entity.Property(a => a.Id).ValueGeneratedNever();

    entity.Property(a => a.FirstName).IsRequired().HasMaxLength(50);

    entity.Property(a => a.LastName).IsRequired().HasMaxLength(50);

    entity.Property(a => a.Email).IsRequired().HasMaxLength(100);

    entity.HasIndex(a => a.Email).IsUnique();
    });

//=======================================================Application entity===============================================================
    modelBuilder.Entity<Application>(entity =>
    {
    entity.ToTable("applications", t =>
    {
        t.HasCheckConstraint(
        "ck_applications_submitted_not_future",
        "\"SubmittedAt\" <= CURRENT_TIMESTAMP");
    });
    
    // composite key of JobListingId and ApplicantId
    entity.HasKey(a => new 
    {
        a.JobListingId,
        a.ApplicantId
    });

    entity.Property(a => a.JobListingId).ValueGeneratedNever();

    entity.Property(a => a.ApplicantId).ValueGeneratedNever();

    entity.Property(a => a.Status).IsRequired();

    entity.Property(a => a.SubmittedAt).IsRequired();

   //
   entity.HasIndex(a => new
    {
        a.ApplicantId,
        a.JobListingId
    })
    .HasDatabaseName("ix_applications_applicantid_joblistingid");

    entity.HasIndex(a => a.JobListingId)
        .HasDatabaseName("ix_applications_joblistingid"); 
});
//=======================================================Relationships===============================================================
    modelBuilder.Entity<JobListing>()
    .HasOne(j => j.Company)
    .WithMany(c => c.JobListings)
    .HasForeignKey(j => j.CompanyId)
    .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Application>()
    .HasOne(a => a.JobListing)
    .WithMany(j => j.Applications)
    .HasForeignKey(a => a.JobListingId)
    .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Application>()
    .HasOne(a => a.Applicant)
    .WithMany(ap => ap.Applications)
    .HasForeignKey(a => a.ApplicantId)
    .OnDelete(DeleteBehavior.Cascade);

}
}