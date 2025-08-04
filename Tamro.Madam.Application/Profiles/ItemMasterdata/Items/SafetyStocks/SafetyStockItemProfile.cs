using AutoMapper;
using Tamro.Madam.Application.Utilities.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.Wholesale;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockItemProfile : Profile
{
    public SafetyStockItemProfile()
    {
        CreateMap<SafetyStockItem, SafetyStockItemModel>()
            .ReverseMap();

        CreateMap<SafetyStockGridData, SafetyStockGridDataModel>();

        CreateMap<WholesaleSafetyStockItem, SafetyStockItemModel>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Country, o => o.Ignore())
            .ForMember(d => d.SafetyStockConditions, o => o.Ignore())
            .ForMember(d => d.SafetyStock, o => o.Ignore())
            .ForMember(d => d.CheckDays, o => o.Ignore());

        CreateMap<WholesaleSafetyStockItem, SafetyStockGridDataModel>()
            .ForMember(d => d.CheckDays, o => o.MapFrom(src => SafetyStockUtility.GetCheckDays(src.ItemGroup)))
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CanBuy, o => o.Ignore())
            .ForMember(d => d.SafetyStockPharmacyChainGroup, o => o.Ignore())
            .ForMember(d => d.PharmacyChainDisplayName, o => o.Ignore())
            .ForMember(d => d.PharmacyChainId, o => o.Ignore())
            .ForMember(d => d.WholesaleQuantity, o => o.Ignore())
            .ForMember(d => d.RetailQuantity, o => o.Ignore())
            .ForMember(d => d.QuantityToBuy, o => o.Ignore())
            .ForMember(d => d.Comment, o => o.Ignore())
            .ForMember(d => d.Country, o => o.Ignore());

        CreateMap<WholesaleSafetyStockItem, WholesaleSafetyStockItemModel>()
            .ForMember(d => d.RtlTransQty, o => o.Ignore());

        CreateMap<SafetyStockGridDataModel, SafetyStockItemModel>()
            .ForMember(d => d.SafetyStock, o => o.Ignore())
            .ForMember(d => d.SafetyStockConditions, o => o.Ignore());

        CreateMap<WholesaleSafetyStockItemModel, SafetyStockItemModel>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Country, o => o.Ignore())
            .ForMember(d => d.SafetyStockConditions, o => o.Ignore())
            .ForMember(d => d.SafetyStock, o => o.Ignore())
            .ForMember(d => d.CheckDays, o => o.Ignore());
    }
}
