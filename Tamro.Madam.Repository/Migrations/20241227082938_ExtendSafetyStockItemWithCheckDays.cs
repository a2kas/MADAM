using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ExtendSafetyStockItemWithCheckDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckDays",
                table: "SafetyStockItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"UPDATE ssi
                                    SET ssi.CheckDays = ssc.CheckDays
                                    FROM [MADAM].[dbo].[SafetyStockItem] ssi
                                    JOIN [MADAM].[dbo].[SafetyStockCondition] ssc 
                                    ON ssc.SafetyStockItemId = ssi.Id;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckDays",
                table: "SafetyStockItem");
        }
    }
}
