using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ExtendItemQualityCheckIssueWithCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "ItemQualityCheckIssue",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "ItemQualityCheckIssue");
        }
    }
}
