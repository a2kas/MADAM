using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CreateItemQualityCheckTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemQualityCheck",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    RowVer = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemQualityCheck", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemQualityCheck_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemQualityCheckIssue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemQualityCheckId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemBindingId = table.Column<int>(type: "int", nullable: true),
                    IssueEntity = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IssueField = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    IssueSeverity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Low"),
                    IssueStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "New"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    RowVer = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemQualityCheckIssue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemQualityCheckIssue_ItemBinding_ItemBindingId",
                        column: x => x.ItemBindingId,
                        principalTable: "ItemBinding",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemQualityCheckIssue_ItemQualityCheck_ItemQualityCheckId",
                        column: x => x.ItemQualityCheckId,
                        principalTable: "ItemQualityCheck",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemQualityCheckIssue_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemQualityCheck_ItemId",
                table: "ItemQualityCheck",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemQualityCheckIssue_ItemBindingId",
                table: "ItemQualityCheckIssue",
                column: "ItemBindingId",
                unique: true,
                filter: "[ItemBindingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ItemQualityCheckIssue_ItemId",
                table: "ItemQualityCheckIssue",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemQualityCheckIssue_ItemQualityCheckId",
                table: "ItemQualityCheckIssue",
                column: "ItemQualityCheckId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemQualityCheckIssue");

            migrationBuilder.DropTable(
                name: "ItemQualityCheck");
        }
    }
}
