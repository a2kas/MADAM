using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations.MadamDb
{
    /// <inheritdoc />
    public partial class Add_NewProductOffer_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewProductOffer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    RowVer = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ItemCategoryManagerId = table.Column<int>(type: "int", nullable: true),
                    FileReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewProductOffer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewProductOffer_ItemCategoryManager_ItemCategoryManagerId",
                        column: x => x.ItemCategoryManagerId,
                        principalTable: "ItemCategoryManager",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewProductOffer_ItemCategoryManagerId",
                table: "NewProductOffer",
                column: "ItemCategoryManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewProductOffer");
        }
    }
}
