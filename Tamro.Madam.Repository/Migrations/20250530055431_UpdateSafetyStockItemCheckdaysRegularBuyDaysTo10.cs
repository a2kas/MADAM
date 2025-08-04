using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSafetyStockItemCheckdaysRegularBuyDaysTo10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update SafetyStockItem set CheckDays = 10 where CheckDays = 14");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Update SafetyStockItem set CheckDays = 14 where CheckDays = 10");
        }
    }
}
