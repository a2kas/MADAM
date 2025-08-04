using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSafetyStockPharmacyChains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "E1SoldTo",
                table: "SafetyStockPharmacyChain",
                type: "int",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.Sql("DELETE FROM SafetyStockPharmacyChain where Value not in ('Benu', 'Camelia', 'Gintarine', 'Medikona', 'Pinkpharma', 'Sks')");
            migrationBuilder.Sql("Update SafetyStockPharmacyChain set E1SoldTo = 20745 where Value = 'Benu'");
            migrationBuilder.Sql("Update SafetyStockPharmacyChain set E1SoldTo = 22614 where Value = 'Camelia'");
            migrationBuilder.Sql("Update SafetyStockPharmacyChain set E1SoldTo = 20094 where Value = 'Gintarine'");
            migrationBuilder.Sql("Update SafetyStockPharmacyChain set E1SoldTo = 21964 where Value = 'Medikona'");
            migrationBuilder.Sql("Update SafetyStockPharmacyChain set E1SoldTo = 26694 where Value = 'Pinkpharma'");
            migrationBuilder.Sql("Update SafetyStockPharmacyChain set E1SoldTo = 0 where Value = 'Sks'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "E1SoldTo",
                table: "SafetyStockPharmacyChain");
        }
    }
}
