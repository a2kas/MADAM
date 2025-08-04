using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddUniquenessConstraintToSafetyStockPharmacyChain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockPharmacyChain_Country_E1SoldTo",
                table: "SafetyStockPharmacyChain",
                columns: new[] { "Country", "E1SoldTo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SafetyStockPharmacyChain_Country_E1SoldTo",
                table: "SafetyStockPharmacyChain");
        }
    }
}
