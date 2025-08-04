using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationshipBetweenItemQualityCheckIssueAndItemBinding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemQualityCheckIssue_ItemBindingId",
                table: "ItemQualityCheckIssue");

            migrationBuilder.CreateIndex(
                name: "IX_ItemQualityCheckIssue_ItemBindingId",
                table: "ItemQualityCheckIssue",
                column: "ItemBindingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemQualityCheckIssue_ItemBindingId",
                table: "ItemQualityCheckIssue");

            migrationBuilder.CreateIndex(
                name: "IX_ItemQualityCheckIssue_ItemBindingId",
                table: "ItemQualityCheckIssue",
                column: "ItemBindingId",
                unique: true,
                filter: "[ItemBindingId] IS NOT NULL");
        }
    }
}
