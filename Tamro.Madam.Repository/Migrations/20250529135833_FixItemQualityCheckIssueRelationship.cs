using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixItemQualityCheckIssueRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemQualityCheckIssue_ItemId",
                table: "ItemQualityCheckIssue");

            migrationBuilder.CreateIndex(
                name: "IX_ItemQualityCheckIssue_ItemId",
                table: "ItemQualityCheckIssue",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemQualityCheckIssue_ItemId",
                table: "ItemQualityCheckIssue");

            migrationBuilder.CreateIndex(
                name: "IX_ItemQualityCheckIssue_ItemId",
                table: "ItemQualityCheckIssue",
                column: "ItemId",
                unique: true);
        }
    }
}
