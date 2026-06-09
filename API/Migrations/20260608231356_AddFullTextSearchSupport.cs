using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddFullTextSearchSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlTsVector>(
                name: "SearchVector",
                table: "job_listings",
                type: "tsvector",
                nullable: true,
                computedColumnSql: "to_tsvector(\r\n            'english',\r\n            coalesce(\"Title\", '') || ' ' ||\r\n            coalesce(\"Description\", '')\r\n        )",
                stored: true);

            migrationBuilder.CreateIndex(
                name: "ix_job_listings_searchvector",
                table: "job_listings",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_job_listings_searchvector",
                table: "job_listings");

            migrationBuilder.DropColumn(
                name: "SearchVector",
                table: "job_listings");
        }
    }
}
