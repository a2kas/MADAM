using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tamro.Madam.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ExtendNewProductOfferItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ActiveSubstance",
                table: "NewProductOfferItem",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgeLimitMax",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgeLimitMin",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlcoholPercentage",
                table: "NewProductOfferItem",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ArgumentsForSale",
                table: "NewProductOfferItem",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Atc",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CipPriceWithoutVat",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "NewProductOfferItem",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompensationBase",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Composition",
                table: "NewProductOfferItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryOfDelivery",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryOfOrigin",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Depth",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "NewProductOfferItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscountCountry",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscountRet",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscountWhsl",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscountWhslAdditional",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FmdSupplierType",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FmdVerificationInboundTamro",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GrossWeight",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "NewProductOfferItem",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HairCondition",
                table: "NewProductOfferItem",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasClinicalResearch",
                table: "NewProductOfferItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ingredients",
                table: "NewProductOfferItem",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsContainingIngredientsOfTfeu",
                table: "NewProductOfferItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCytostatic",
                table: "NewProductOfferItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsForEcommerce",
                table: "NewProductOfferItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHomeopathy",
                table: "NewProductOfferItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLabelingOrLeafletingNeeded",
                table: "NewProductOfferItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotificationDoneInCpnp",
                table: "NewProductOfferItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReimbursed",
                table: "NewProductOfferItem",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "NewProductOfferItem",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LifeStage",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainProductCompetitors",
                table: "NewProductOfferItem",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainSalesChannel",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketingActivitiesInBenu",
                table: "NewProductOfferItem",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketingActivitiesInTv",
                table: "NewProductOfferItem",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketingAuthorizationHolder",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarketingInvestmentInBenu",
                table: "NewProductOfferItem",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaximumShelfLifeInDays",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Measure",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicineType",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "NewProductOfferItem",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NarcoticsOrPsychotropics",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomenclatureNumber",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NpakId7",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "NewProductOfferItem",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherInformation",
                table: "NewProductOfferItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageOrLabelLanguage",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageQty",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageQtyInSecondaryUnit",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageQtyInTransportUnit",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientInformationLeafletLanguage",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Producer",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProducerAddress",
                table: "NewProductOfferItem",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProducerEmail",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductInformationLeafletReference",
                table: "NewProductOfferItem",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommendedSalesPriceInPharmacy",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegistrationNo",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReimbursedFrom",
                table: "NewProductOfferItem",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponsibleContact",
                table: "NewProductOfferItem",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "NewProductOfferItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkinCondition",
                table: "NewProductOfferItem",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialProperties",
                table: "NewProductOfferItem",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecialStorage",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StoringTemperature",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Strength",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "NewProductOfferItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierItemCode",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitOfMeasure",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Usage",
                table: "NewProductOfferItem",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vat",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarningsAndSafetyInstructions",
                table: "NewProductOfferItem",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarningsAndSafetySigns",
                table: "NewProductOfferItem",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Width",
                table: "NewProductOfferItem",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveSubstance",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "AgeLimitMax",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "AgeLimitMin",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "AlcoholPercentage",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "ArgumentsForSale",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Atc",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "CipPriceWithoutVat",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Class",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "CompensationBase",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Composition",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "CountryOfDelivery",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "CountryOfOrigin",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Depth",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "DiscountCountry",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "DiscountRet",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "DiscountWhsl",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "DiscountWhslAdditional",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "FmdSupplierType",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "FmdVerificationInboundTamro",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "GrossWeight",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "HairCondition",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "HasClinicalResearch",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Ingredients",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "IsContainingIngredientsOfTfeu",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "IsCytostatic",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "IsForEcommerce",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "IsHomeopathy",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "IsLabelingOrLeafletingNeeded",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "IsNotificationDoneInCpnp",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "IsReimbursed",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "LifeStage",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "MainProductCompetitors",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "MainSalesChannel",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "MarketingActivitiesInBenu",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "MarketingActivitiesInTv",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "MarketingAuthorizationHolder",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "MarketingInvestmentInBenu",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "MaximumShelfLifeInDays",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Measure",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "MedicineType",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "NarcoticsOrPsychotropics",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "NomenclatureNumber",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "NpakId7",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "OtherInformation",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "PackageOrLabelLanguage",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "PackageQty",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "PackageQtyInSecondaryUnit",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "PackageQtyInTransportUnit",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "PatientInformationLeafletLanguage",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Producer",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "ProducerAddress",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "ProducerEmail",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "ProductInformationLeafletReference",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "RecommendedSalesPriceInPharmacy",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "RegistrationNo",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "ReimbursedFrom",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "ResponsibleContact",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "SkinCondition",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "SpecialProperties",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "SpecialStorage",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "StoringTemperature",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Strength",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "SupplierItemCode",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Usage",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Vat",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "WarningsAndSafetyInstructions",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "WarningsAndSafetySigns",
                table: "NewProductOfferItem");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "NewProductOfferItem");
        }
    }
}
