using System.Collections.Immutable;

namespace Tamro.Madam.Models.Data;

public static class AuditData
{
    public static ImmutableDictionary<string, string> AuditEntryDisplayNames { get; } = ImmutableDictionary<string, string>.Empty
        .Add("Atc", "ATC")
        .Add("Item", "Baltic item")
        .Add("Barcode", "Barcode")
        .Add("Brand", "Brand")
        .Add("Form", "Form")
        .Add("GeneratedRetailCode", "Generated retail code")
        .Add("ItemBinding", "Local item")
        .Add("ItemBindingLanguage", "Local item language")
        .Add("ItemAssortmentSalesChannel", "Item assortment sales channel")
        .Add("MeasurementUnit", "Measurement unit")
        .Add("Nick", "Nick")
        .Add("Producer", "Producer")
        .Add("Requestor", "Requestor")
        .Add("SafetyStockCondition", "Safety stock condition")
        .Add("SafetyStockItem", "Safety stock item")
        .Add("SafetyStockPharmacyChain", "Safety stock pharmacy chain")
        .Add("SafetyStock", "Safety stock quantity")
        .Add("UserSetting", "User setting")
        .Add("VlkBinding", "Vlk binding")
        .Add("CustomerLegalEntity", "Customer legal entity")
        .Add("CustomerLegalEntityNotification", "Customer legal entity notification settings")
        .Add("Supplier", "Supplier")
        .Add("SupplierContract", "Supplier contract")
        .Add("CategoryManager", "Category manager")
        .Add("NewProductOffer", "New product offer")
        .Add("NewProductOfferComment", "New product offer comment")
        .Add("NewProductOfferItem", "New product offer item")
        .Add("NewProductOfferAttachment", "New product offer attachment")
        .Add("SkuForm", "SKU form");

    public static ImmutableDictionary<string, string> AuditStateDisplayNames { get; } = ImmutableDictionary<string, string>.Empty
        .Add("EntitySoftAdded", "Activated")
        .Add("EntityAdded", "Created")
        .Add("EntityDeleted", "Deleted")
        .Add("EntitySoftDeleted", "Inactivated")
        .Add("RelationshipAdded", "Linked")
        .Add("EntityModified", "Modified")
        .Add("EntityCurrent", "Other")
        .Add("RelationshipDeleted", "Unlinked");
}
