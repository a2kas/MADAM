using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquenessConstraintOnItemAssortmentSalesChannel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ItemAssortmentSalesChannel_Country_Name",
                table: "ItemAssortmentSalesChannel",
                columns: new[] { "Country", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemAssortmentSalesChannel_Country_Name",
                table: "ItemAssortmentSalesChannel");
        }
    }
}
