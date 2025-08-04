namespace Tamro.Madam.Application.Access;

public static class Permissions
{
    public const string CanEditCoreMasterdata = "Masterdata.Edit";
    public const string CanViewCoreMasterdata = "Masterdata.View";
    public const string CanManageSafetyStock = "Sales.SafetyStock.Manage";
    public const string CanViewSafetyStock = "Sales.SafetyStock.View";
    public const string CanViewAudit = "General.Audit.View";
    public const string CanManageSettings = "Administrator.Settings.Manage";
    public const string CanManageJobs = "Administrator.Jobs.Manage";
    public const string CanAccessDevelopersCorner = "Developer.View";
    public const string CanManageVlkBindings = "Sales.VlkBindings.Manage";
    public const string CanViewCanceledOrderLines = "Sales.CanceledOrderLines.View";
    public const string CanManageCanceledOrderLineNotificationReceivers = "Sales.CanceledOrderLines.NotificationReceivers.Manage";
    public const string CanManageSabisContracts = "Sales.Sabis.Contracts.Manage";
    public const string CanViewSabisContracts = "Sales.Sabis.Contracts.View";
    public const string CanViewHeldOrders = "Sales.HeldOrders.View";
    public const string CanViewItemAssortmentSalesChannels = "Commerce.ItemAssortmentSalesChannel.View";
    public const string CanManageItemAssortmentSalesChannels = "Commerce.ItemAssortmentSalesChannel.Modify";
    public const string CanGenerateDynamicsImport = "Hr.Dynamics.Import.Generate";
    public const string CanViewPeppol = "Finance.Peppol.View";
    public const string CanViewJira = "Hr.Jira.View";
    public const string CanManageJira = "Hr.Jira.Manage";
    public const string CanManageUnifiedPost = "Administration.UnifiedPost.Manage";
    public const string CountryLv = "Country.LV";
    public const string CountryLt = "Country.LT";
    public const string CountryEe = "Country.EE";
    public const string CanViewProductOffers = "Commerce.ProductOffers.View";
}
