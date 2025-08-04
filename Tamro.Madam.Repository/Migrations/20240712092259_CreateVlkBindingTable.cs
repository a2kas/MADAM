using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CreateVlkBindingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VlkBinding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NpakId7 = table.Column<int>(type: "int", nullable: false),
                    ItemBindingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VlkBinding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VlkBinding_ItemBinding_ItemBindingId",
                        column: x => x.ItemBindingId,
                        principalTable: "ItemBinding",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VlkBinding_ItemBindingId",
                table: "VlkBinding",
                column: "ItemBindingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VlkBinding");
        }
    }
}
