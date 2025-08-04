using System.ComponentModel;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOfferItems;
using Tamro.Madam.Repository.Context.Madam;
using TamroUtilities.EFCore.Models;

namespace Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;

[DisplayName("New product offer item")]
public class NewProductOfferItem : IMadamEntity<int>, IAuditable, IBaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime RowVer { get; set; }

    public required int NewProductOfferId { get; set; }
    public required string Name { get; set; }
    public required string NameEn { get; set; }
    public string? Group { get; set; }
    public string? Class { get; set; }
    public string? Atc { get; set; }
    public string? Barcode { get; set; }
    public string? RegistrationNo { get; set; }
    public string? Strength { get; set; }
    public string? Measure { get; set; }
    public string? UnitOfMeasure { get; set; }
    public string? Numero { get; set; }
    public string? Brand { get; set; }
    public bool? HasClinicalResearch { get; set; }
    public bool? IsHomeopathy { get; set; }
    public string? ActiveSubstance { get; set; }
    public string? StoringTemperature { get; set; }
    public string? SpecialStorage { get; set; }
    public string? PackageOrLabelLanguage { get; set; }
    public string? PatientInformationLeafletLanguage { get; set; }
    public string? Height { get; set; }
    public string? Width { get; set; }
    public string? Depth { get; set; }
    public string? GrossWeight { get; set; }
    public string? PackageQty { get; set; }
    public string? PackageQtyInSecondaryUnit { get; set; }
    public string? PackageQtyInTransportUnit { get; set; }
    public string? MaximumShelfLifeInDays { get; set; }
    public string? CipPriceWithoutVat { get; set; }
    public string? DiscountWhsl { get; set; }
    public string? DiscountWhslAdditional { get; set; }
    public string? DiscountCountry { get; set; }
    public string? DiscountRet { get; set; }
    public string? Vat { get; set; }
    public string? Supplier { get; set; }
    public string? SupplierItemCode { get; set; }
    public string? CountryOfOrigin { get; set; }
    public string? CountryOfDelivery { get; set; }
    public string? Producer { get; set; }
    public string? ProducerAddress { get; set; }
    public string? ProducerEmail { get; set; }
    public string? ResponsibleContact { get; set; }
    public bool? IsForEcommerce { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? ProductInformationLeafletReference { get; set; }
    public string? Composition { get; set; }
    public string? Usage { get; set; }
    public string? Keywords { get; set; }
    public string? SkinCondition { get; set; }
    public string? HairCondition { get; set; }
    public string? SpecialProperties { get; set; }
    public string? AgeLimitMin { get; set; }
    public string? AgeLimitMax { get; set; }
    public string? LifeStage { get; set; }
    public string? OtherInformation { get; set; }
    public string? WarningsAndSafetyInstructions { get; set; }
    public string? WarningsAndSafetySigns { get; set; }
    public bool? IsReimbursed { get; set; }
    public string? ReimbursedFrom { get; set; }
    public string? NarcoticsOrPsychotropics { get; set; }
    public string? MedicineType { get; set; }
    public bool? IsCytostatic { get; set; }
    public string? NpakId7 { get; set; }
    public string? Ingredients { get; set; }
    public bool? IsContainingIngredientsOfTfeu { get; set; }
    public bool? IsNotificationDoneInCpnp { get; set; }
    public string? AlcoholPercentage { get; set; }
    public bool? IsLabelingOrLeafletingNeeded { get; set; }
    public string? NomenclatureNumber { get; set; }
    public string? RecommendedSalesPriceInPharmacy { get; set; }
    public string? CompensationBase { get; set; }
    public string? FmdVerificationInboundTamro { get; set; }
    public string? FmdSupplierType { get; set; }
    public string? MarketingAuthorizationHolder { get; set; }
    public string? MainSalesChannel { get; set; }
    public string? MarketingActivitiesInTv { get; set; }
    public string? MarketingActivitiesInBenu { get; set; }
    public string? MarketingInvestmentInBenu { get; set; }
    public string? MainProductCompetitors { get; set; }
    public string? ArgumentsForSale { get; set; }
    public required NewProductOfferItemStatus Status { get; set; }

    public virtual required NewProductOffer NewProductOffer { get; set; }
}
