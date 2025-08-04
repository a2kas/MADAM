using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Column_Mapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVer",
                table: "SafetyStockCondition",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.Sql("ALTER TABLE SafetyStockCondition ADD TempDateTime datetime2;");
            migrationBuilder.Sql("UPDATE SafetyStockCondition SET TempDateTime = RowVer;");
            migrationBuilder.Sql("UPDATE SafetyStockCondition SET RowVer = CreatedDate;");
            migrationBuilder.Sql("UPDATE SafetyStockCondition SET CreatedDate = TempDateTime;");
            migrationBuilder.Sql("ALTER TABLE SafetyStockCondition DROP COLUMN TempDateTime;");       
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVer",
                table: "SafetyStockCondition",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
