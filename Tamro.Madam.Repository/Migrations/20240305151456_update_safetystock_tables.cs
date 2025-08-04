using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class update_safetystock_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStockCondition_SafetyStockItem_SafetyStockItemId",
                table: "SafetyStockCondition");

            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStockCondition_SafetyStockPharmacyChain_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition");

            migrationBuilder.DropIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition");

            migrationBuilder.RenameColumn(
                name: "ItemDescription",
                table: "SafetyStockItem",
                newName: "ItemName");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SafetyStockPharmacyChain",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "SafetyStockPharmacyChainId",
                table: "SafetyStockCondition",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "RestrictionLevel",
                table: "SafetyStockCondition",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SafetyStockPharmacyChainGroup",
                table: "SafetyStockCondition",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockPharmacyChain_Country_Value",
                table: "SafetyStockPharmacyChain",
                columns: new[] { "Country", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition",
                columns: new[] { "SafetyStockItemId", "SafetyStockPharmacyChainId" },
                unique: true,
                filter: "[SafetyStockPharmacyChainId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStockCondition_SafetyStockItem_SafetyStockItemId",
                table: "SafetyStockCondition",
                column: "SafetyStockItemId",
                principalTable: "SafetyStockItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStockCondition_SafetyStockPharmacyChain_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition",
                column: "SafetyStockPharmacyChainId",
                principalTable: "SafetyStockPharmacyChain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStockCondition_SafetyStockItem_SafetyStockItemId",
                table: "SafetyStockCondition");

            migrationBuilder.DropForeignKey(
                name: "FK_SafetyStockCondition_SafetyStockPharmacyChain_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition");

            migrationBuilder.DropIndex(
                name: "IX_SafetyStockPharmacyChain_Country_Value",
                table: "SafetyStockPharmacyChain");

            migrationBuilder.DropIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SafetyStockPharmacyChain");

            migrationBuilder.DropColumn(
                name: "RestrictionLevel",
                table: "SafetyStockCondition");

            migrationBuilder.DropColumn(
                name: "SafetyStockPharmacyChainGroup",
                table: "SafetyStockCondition");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "SafetyStockItem",
                newName: "ItemDescription");

            migrationBuilder.AlterColumn<int>(
                name: "SafetyStockPharmacyChainId",
                table: "SafetyStockCondition",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition",
                columns: new[] { "SafetyStockItemId", "SafetyStockPharmacyChainId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStockCondition_SafetyStockItem_SafetyStockItemId",
                table: "SafetyStockCondition",
                column: "SafetyStockItemId",
                principalTable: "SafetyStockItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SafetyStockCondition_SafetyStockPharmacyChain_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition",
                column: "SafetyStockPharmacyChainId",
                principalTable: "SafetyStockPharmacyChain",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
