using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "job_listings");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "job_listings",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "applicants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "applications",
                columns: table => new
                {
                    JobListingId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ApplicantId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applications", x => new { x.JobListingId, x.ApplicantId });
                    table.ForeignKey(
                        name: "FK_applications_applicants_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "applicants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_applications_job_listings_JobListingId",
                        column: x => x.JobListingId,
                        principalTable: "job_listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_job_listings_CompanyId",
                table: "job_listings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_applicants_Email",
                table: "applicants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_applications_ApplicantId",
                table: "applications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_companies_Name",
                table: "companies",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_job_listings_companies_CompanyId",
                table: "job_listings",
                column: "CompanyId",
                principalTable: "companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_job_listings_companies_CompanyId",
                table: "job_listings");

            migrationBuilder.DropTable(
                name: "applications");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "applicants");

            migrationBuilder.DropIndex(
                name: "IX_job_listings_CompanyId",
                table: "job_listings");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "job_listings");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "job_listings",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
