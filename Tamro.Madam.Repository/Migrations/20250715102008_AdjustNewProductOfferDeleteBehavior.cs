using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AdjustNewProductOfferDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewProductOffer_ItemCategoryManager_ItemCategoryManagerId",
                table: "NewProductOffer");

            migrationBuilder.AddForeignKey(
                name: "FK_NewProductOffer_ItemCategoryManager_ItemCategoryManagerId",
                table: "NewProductOffer",
                column: "ItemCategoryManagerId",
                principalTable: "ItemCategoryManager",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewProductOffer_ItemCategoryManager_ItemCategoryManagerId",
                table: "NewProductOffer");

            migrationBuilder.AddForeignKey(
                name: "FK_NewProductOffer_ItemCategoryManager_ItemCategoryManagerId",
                table: "NewProductOffer",
                column: "ItemCategoryManagerId",
                principalTable: "ItemCategoryManager",
                principalColumn: "Id");
        }
    }
}
