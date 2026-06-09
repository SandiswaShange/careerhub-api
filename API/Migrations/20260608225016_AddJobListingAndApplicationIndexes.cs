using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddJobListingAndApplicationIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_job_listings_CompanyId",
                table: "job_listings");

            migrationBuilder.DropIndex(
                name: "IX_applications_ApplicantId",
                table: "applications");

            migrationBuilder.CreateIndex(
                name: "ix_job_listings_companyid_isactive",
                table: "job_listings",
                columns: new[] { "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "ix_job_listings_isactive_closingdate",
                table: "job_listings",
                columns: new[] { "IsActive", "ClosingDate" });

            migrationBuilder.CreateIndex(
                name: "ix_applications_applicantid_joblistingid",
                table: "applications",
                columns: new[] { "ApplicantId", "JobListingId" });

            migrationBuilder.CreateIndex(
                name: "ix_applications_joblistingid",
                table: "applications",
                column: "JobListingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_job_listings_companyid_isactive",
                table: "job_listings");

            migrationBuilder.DropIndex(
                name: "ix_job_listings_isactive_closingdate",
                table: "job_listings");

            migrationBuilder.DropIndex(
                name: "ix_applications_applicantid_joblistingid",
                table: "applications");

            migrationBuilder.DropIndex(
                name: "ix_applications_joblistingid",
                table: "applications");

            migrationBuilder.CreateIndex(
                name: "IX_job_listings_CompanyId",
                table: "job_listings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_applications_ApplicantId",
                table: "applications",
                column: "ApplicantId");
        }
    }
}
