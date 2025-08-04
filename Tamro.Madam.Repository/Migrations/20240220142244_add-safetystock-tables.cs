using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addsafetystocktables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SafetyStockItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    ItemNo2 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ItemGroup = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    ProductClass = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true),
                    SupplierNumber = table.Column<int>(type: "int", nullable: true),
                    SupplierNick = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Cn3 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cn1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Substance = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStockItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStockPharmacyChain",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Group = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStockPharmacyChain", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStock",
                columns: table => new
                {
                    SafetyStockItemId = table.Column<int>(type: "int", nullable: false),
                    WholesaleQuantity = table.Column<int>(type: "int", nullable: true),
                    RetailQuantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStock", x => x.SafetyStockItemId);
                    table.ForeignKey(
                        name: "FK_SafetyStock_SafetyStockItem_SafetyStockItemId",
                        column: x => x.SafetyStockItemId,
                        principalTable: "SafetyStockItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SafetyStockCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SafetyStockItemId = table.Column<int>(type: "int", nullable: false),
                    SafetyStockPharmacyChainId = table.Column<int>(type: "int", nullable: false),
                    CanBuy = table.Column<bool>(type: "bit", nullable: false),
                    CheckDays = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    User = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    RowVer = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafetyStockCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SafetyStockCondition_SafetyStockItem_SafetyStockItemId",
                        column: x => x.SafetyStockItemId,
                        principalTable: "SafetyStockItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SafetyStockCondition_SafetyStockPharmacyChain_SafetyStockPharmacyChainId",
                        column: x => x.SafetyStockPharmacyChainId,
                        principalTable: "SafetyStockPharmacyChain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockCondition_SafetyStockItemId_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition",
                columns: new[] { "SafetyStockItemId", "SafetyStockPharmacyChainId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockCondition_SafetyStockPharmacyChainId",
                table: "SafetyStockCondition",
                column: "SafetyStockPharmacyChainId");

            migrationBuilder.CreateIndex(
                name: "IX_SafetyStockItem_ItemNo2_Country",
                table: "SafetyStockItem",
                columns: new[] { "ItemNo2", "Country" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SafetyStock");

            migrationBuilder.DropTable(
                name: "SafetyStockCondition");

            migrationBuilder.DropTable(
                name: "SafetyStockItem");

            migrationBuilder.DropTable(
                name: "SafetyStockPharmacyChain");
        }
    }
}
