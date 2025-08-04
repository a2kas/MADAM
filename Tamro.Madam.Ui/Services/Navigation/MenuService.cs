using MudBlazor;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Models.Navigation.Menu;

namespace Tamro.Madam.Ui.Services.Navigation;

public class MenuService : IMenuService
{
    private readonly List<MenuSectionModel> _features =
    [
        new MenuSectionModel {
            Title = "Masterdata",
            SectionItems = new List<MenuSectionItemModel> 
            {
                new()
                {
                    Title = "Items",
                    IsParent = true,
                    Icon = Icons.Material.Filled.MedicationLiquid,
                    Permissions = [Permissions.CanViewCoreMasterdata],
                    MenuItems = [
                        new()
                        {
                            Title = "ATC's",
                            IsParent = false,
                            Href = "/items/atcs",
                            Icon = Icons.Material.Outlined.AutoStories,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                        new()
                        {
                            Title = "Baltic nicks",
                            IsParent = false,
                            Href = "/items/nicks",
                            Icon = Icons.Material.Outlined.Group,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                        new()
                        {
                            Title = "Barcodes",
                            IsParent = false,
                            Href = "/items/barcodes",
                            Icon = Icons.Material.Outlined.QrCodeScanner,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                        new()
                        {
                            Title = "Brands",
                            IsParent = false,
                            Href = "/items/brands",
                            Icon = Icons.Material.Outlined.Ballot,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                        new()
                        {
                            Title = "Category managers",
                            IsParent = false,
                            Href = "/items/category-managers",
                            Icon = Icons.Material.Outlined.People,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                        new()
                        {
                            Title = "Forms",
                            IsParent = false,
                            Href = "/items/forms",
                            Icon = Icons.Material.Outlined.AllOut,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                        new()
                        {
                            Title = "Items",
                            IsParent = false,
                            Href = "/items",
                            Icon = Icons.Material.Outlined.MedicationLiquid,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                        new()
                        {
                            Title = "Measurement units",
                            IsParent = false,
                            Href = "/items/measurement-units",
                            Icon = Icons.Material.Outlined.Scale,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                        new()
                        {
                            Title = "Producers",
                            IsParent = false,
                            Href = "/items/producers",
                            Icon = Icons.Material.Outlined.Factory,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                        new()
                        {
                            Title = "Responsible persons",
                            IsParent = false,
                            Href = "/items/responsible-persons",
                            Icon = Icons.Material.Outlined.PersonOutline,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                    ]
                },
                new()
                {
                    Title = "Suppliers",
                    IsParent = true,
                    Icon = Icons.Material.Filled.Business,
                    Permissions = [Permissions.CanViewCoreMasterdata],
                    MenuItems = [
                        new()
                        {
                            Title = "Suppliers",
                            IsParent = false,
                            Href = "/suppliers",
                            Icon = Icons.Material.Outlined.Business,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                     ]
                },
            }
        },
        new MenuSectionModel
        {
            Title = "Menu",
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    Title = "AI assistants",
                    IsParent = true,
                    Icon = Icons.Material.Filled.RocketLaunch,
                    MenuItems = [
                        new () {
                            Title = "Item masterdata quality check",
                            IsParent = false,
                            Href = "/items/item-quality-check",
                            Icon = Icons.Material.Outlined.Check,
                            Permissions = [Permissions.CanViewCoreMasterdata],
                        },
                    ]
                },
                new()
                {
                    Title = "Commerce",
                    IsParent = true,
                    Icon = Icons.Material.Filled.LocalMall,
                    MenuItems = [
                        new()
                        {
                            Title = "Item assortment sales channels",
                            IsParent = false,
                            Href = "/commerce/item-assortment-sales-channels",
                            Icon = Icons.Material.Outlined.LocalOffer,
                            Permissions = [Permissions.CanViewItemAssortmentSalesChannels],
                        },
                    ]
                },
                new()
                {
                    Title = "Finance",
                    IsParent = true,
                    Icon = Icons.Material.Filled.AttachMoney,
                    MenuItems = [
                        new()
                        {
                            Title = "Peppol",
                            IsParent = false,
                            Href = "/finance/peppol",
                            Icon = Icons.Material.Outlined.DocumentScanner,
                            Permissions = [Permissions.CanViewPeppol],
                        },
                    ]
                },
                new()
                {
                    Title = "Human resources",
                    IsParent = true,
                    Icon = Icons.Material.Filled.Group,
                    MenuItems = [
                        new()
                        {
                            Title = "Dynamics FO import",
                            IsParent = false,
                            Href = "/hr/dynamics-fo-import",
                            Icon = Icons.Material.Outlined.ImportContacts,
                            Permissions = [Permissions.CanGenerateDynamicsImport],
                        },
                        new()
                        {
                            Title = "Jira administration",
                            IsParent = false,
                            Href = "/hr/jira-administration",
                            Icon = Icons.Material.Outlined.Face5,
                            Permissions = [Permissions.CanViewJira],
                        },
                    ],
                },
                new()
                {
                    Title = "Sales",
                    IsParent = true,
                    Icon = Icons.Material.Filled.AttachMoney,
                    MenuItems =
                    [
                        new()
                        {
                            Title = "Canceled order lines",
                            Icon = Icons.Material.Filled.DeleteSweep,
                            IsParent = true,
                            MenuItems = [
                                new()
                                {
                                    Title = "Canceled order lines",
                                    Icon = Icons.Material.Outlined.DeleteSweep,
                                    Permissions = [Permissions.CanViewCanceledOrderLines],
                                    Href = "/sales/canceled-order-lines"
                                },
                                new()
                                {
                                    Title = "Excluded customers",
                                    Icon = Icons.Material.Outlined.PlaylistRemove,
                                    Permissions = [Permissions.CanManageCanceledOrderLineNotificationReceivers],
                                    Href = "/sales/canceled-order-lines/excluded-customers"
                                },
                                new()
                                {
                                    Title = "Statistics",
                                    Icon = Icons.Material.Outlined.SsidChart,
                                    Permissions = [Permissions.CanViewCanceledOrderLines],
                                    Href = "/sales/canceled-order-lines/statistics"
                                }
                            ]
                        },
                        new(){
                            Title = "Held orders",
                            Icon = Icons.Material.Outlined.FreeCancellation,
                            Permissions = [Permissions.CanViewHeldOrders],
                            Href = "/sales/held-orders",
                        },
                        new() {
                            Title = "SABIS",
                            Icon = Icons.Material.Outlined.LibraryBooks,
                            Permissions= [Permissions.CanViewSabisContracts],
                            Href = "/sales/sabis"
                        },
                        new()
                        {
                            Title = "Vlk bindings",
                            Icon = Icons.Material.Outlined.Link,
                            Permissions = [Permissions.CanManageVlkBindings],
                            Href = "/sales/items/vlk-bindings",
                        },
                    ]
                },
                new()
                {
                    Title = "Tools",
                    IsParent = true,
                    Icon = Icons.Material.Filled.Build,
                    MenuItems =
                    [
                        new()
                        {
                            Title = "Retail code generator",
                            Icon = Icons.Material.Outlined.AutoMode,
                            Permissions = [Permissions.CanEditCoreMasterdata],
                            Href = "/tools/retail-code-generator"
                        }
                    ]
                },
                new()
                {
                    Title = "Safety stock",
                    Icon = Icons.Material.Filled.Beenhere,
                    IsParent = true,
                    MenuItems =
                    [
                        new()
                        {
                            Title = "Pharmacy chains",
                            Href = "/safety-stock/pharmacy-chains",
                            Icon = Icons.Material.Outlined.LocalPharmacy,
                            Permissions = [Permissions.CanViewSafetyStock],
                        },
                        new()
                        {
                            Title = "Safety stock",
                            Href = "/safety-stock/safety-stocks",
                            Icon = Icons.Material.Outlined.Beenhere,
                            Permissions = [Permissions.CanViewSafetyStock],
                        }
                    ]
                }
            }.OrderBy(item => item.Title).ToList()
        },
        new MenuSectionModel
        {
            Title = "Administration",
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    Title = "Audit",
                    Icon = Icons.Material.Outlined.TrackChanges,
                    Href = "/audit",
                    Permissions = [Permissions.CanViewAudit],
                },
                new()
                {
                    Title = "Settings",
                    Icon = Icons.Material.Outlined.Settings,
                    Href = "/settings",
                    Permissions = [Permissions.CanManageSettings],
                },
                new()
                {
                    Title = "Synchronization jobs",
                    Icon = Icons.Material.Outlined.Task,
                    Href = "/administration/jobs",
                    Permissions = [Permissions.CanManageJobs],
                },
                new(){
                    Title = "Configuration",
                    Icon = Icons.Material.Outlined.SettingsInputComposite,
                    IsParent = true,
                    MenuItems =
                    [
                        new()
                        {
                            Title = "UnifiedPost API keys",
                            Icon = Icons.Material.Outlined.VpnKey,
                            Href = "/administration/configuration/unifiedpostapikeys",
                            Permissions = [Permissions.CanManageUnifiedPost],
                        }
                    ],
                }

            }.OrderBy(item => item.Title).ToList()
        },
        new MenuSectionModel
        {
            Title = "Developers corner",
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    Title = "Samples",
                    Icon = Icons.Material.Filled.DeveloperMode,
                    IsParent = true,
                    MenuItems =
                    [
                        new()
                        {
                            Title = "ELK",
                            Href = "/dev/samples/elk",
                            Permissions = [Permissions.CanAccessDevelopersCorner],
                        },
                        new()
                        {
                            Title = "Environment variables",
                            Href = "/dev/samples/environmentvariable",
                            Permissions = [Permissions.CanAccessDevelopersCorner],
                        }
                    ]
                }
            }.OrderBy(item => item.Title).ToList()
        }
    ];

    public IEnumerable<MenuSectionModel> Features => _features;
}
