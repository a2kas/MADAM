using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ExtendSafetyStockConditionConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainGroup",
                table: "SafetyStockCondition",
                columns: new[] { "SafetyStockItemId", "SafetyStockPharmacyChainGroup" },
                unique: true,
                filter: "(SafetyStockPharmacyChainId IS NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainId_SafetyStockPharmacyChainGroup",
                table: "SafetyStockCondition",
                columns: new[] { "SafetyStockItemId", "SafetyStockPharmacyChainId", "SafetyStockPharmacyChainGroup" },
                unique: true,
                filter: "(SafetyStockPharmacyChainId IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainGroup",
                table: "SafetyStockCondition");

            migrationBuilder.DropIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainId_SafetyStockPharmacyChainGroup",
                table: "SafetyStockCondition");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition",
                columns: new[] { "SafetyStockItemId", "SafetyStockPharmacyChainId" },
                unique: true,
                filter: "[SafetyStockPharmacyChainId] IS NOT NULL");
        }
    }
}
