using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ExtendSupplierWithCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Supplier",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Supplier");
        }
    }
}
