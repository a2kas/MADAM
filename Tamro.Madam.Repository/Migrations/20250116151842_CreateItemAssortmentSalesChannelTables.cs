using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CreateItemAssortmentSalesChannelTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemAssortmentSalesChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    RowVer = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAssortmentSalesChannel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemAssortmentBindingMap",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemBindingId = table.Column<int>(type: "int", nullable: false),
                    ItemAssortmentSalesChannelId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    RowVer = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemAssortmentBindingMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemAssortmentBindingMap_ItemAssortmentSalesChannel_ItemAssortmentSalesChannelId",
                        column: x => x.ItemAssortmentSalesChannelId,
                        principalTable: "ItemAssortmentSalesChannel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemAssortmentBindingMap_ItemBinding_ItemBindingId",
                        column: x => x.ItemBindingId,
                        principalTable: "ItemBinding",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemAssortmentBindingMap_ItemAssortmentSalesChannelId",
                table: "ItemAssortmentBindingMap",
                column: "ItemAssortmentSalesChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemAssortmentBindingMap_ItemBindingId_ItemAssortmentSalesChannelId",
                table: "ItemAssortmentBindingMap",
                columns: new[] { "ItemBindingId", "ItemAssortmentSalesChannelId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemAssortmentBindingMap");

            migrationBuilder.DropTable(
                name: "ItemAssortmentSalesChannel");
        }
    }
}
