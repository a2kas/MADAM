using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Delete_SafetyStockPharmacyChain_Value_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
             name: "IX_SafetyStockPharmacyChain_Country_Value",
             table: "SafetyStockPharmacyChain");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "SafetyStockPharmacyChain");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "SafetyStockPharmacyChain",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockPharmacyChain_Country_Value",
                table: "SafetyStockPharmacyChain",
                columns: new[] { "Country", "Value" },
                unique: true);
        }
    }
}
