using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseCheckConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "ck_job_listings_closing_after_posted",
                table: "job_listings",
                sql: "\"ClosingDate\" > \"PostedAt\"");

            migrationBuilder.AddCheckConstraint(
                name: "ck_job_listings_min_salary_positive",
                table: "job_listings",
                sql: "\"MinSalary\" IS NULL OR \"MinSalary\" > 0");

            migrationBuilder.AddCheckConstraint(
                name: "ck_job_listings_salary_range",
                table: "job_listings",
                sql: "\"MinSalary\" IS NULL OR \"MaxSalary\" IS NULL OR \"MaxSalary\" > \"MinSalary\"");

            migrationBuilder.AddCheckConstraint(
                name: "ck_applications_submitted_not_future",
                table: "applications",
                sql: "\"SubmittedAt\" <= CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_job_listings_closing_after_posted",
                table: "job_listings");

            migrationBuilder.DropCheckConstraint(
                name: "ck_job_listings_min_salary_positive",
                table: "job_listings");

            migrationBuilder.DropCheckConstraint(
                name: "ck_job_listings_salary_range",
                table: "job_listings");

            migrationBuilder.DropCheckConstraint(
                name: "ck_applications_submitted_not_future",
                table: "applications");
        }
    }
}
