using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockProfile : Profile
{
    public SafetyStockProfile()
    {
        CreateMap<SafetyStock, SafetyStockModel>()
            .ForMember(d => d.QtyToBuy, o => o.Ignore())
            .ReverseMap();

        CreateMap<SafetyStockGridDataModel, SafetyStockConditionUpsertModel>()
            .ForMember(d => d.PharmacyGroup, o => o.MapFrom(src => src.SafetyStockPharmacyChainGroup))
            .ForMember(d => d.PharmacyChainName, o => o.MapFrom(src => src.PharmacyChainDisplayName));

        CreateMap<SafetyStockGridDataModel, SafetyStockConditionEditDialogModel>();

        CreateMap<SafetyStockConditionUpsertModel, SafetyStockGridDataModel>()
            .ForMember(d => d.SafetyStockPharmacyChainGroup, o => o.MapFrom(src => src.PharmacyGroup.ToString()))
            .ForMember(d => d.PharmacyChainDisplayName, o => o.MapFrom(src => src.PharmacyChainName))
            .ForMember(d => d.ItemNo, o => o.Ignore())
            .ForMember(d => d.ItemName, o => o.Ignore())
            .ForMember(d => d.WholesaleQuantity, o => o.Ignore())
            .ForMember(d => d.RetailQuantity, o => o.Ignore())
            .ForMember(d => d.QuantityToBuy, o => o.Ignore())
            .ForMember(d => d.ItemGroup, o => o.Ignore())
            .ForMember(d => d.ProductClass, o => o.Ignore())
            .ForMember(d => d.Brand, o => o.Ignore())
            .ForMember(d => d.SupplierNumber, o => o.Ignore())
            .ForMember(d => d.SupplierNick, o => o.Ignore())
            .ForMember(d => d.Cn3, o => o.Ignore())
            .ForMember(d => d.Cn1, o => o.Ignore())
            .ForMember(d => d.Substance, o => o.Ignore())
            .ForMember(d => d.Country, o => o.Ignore());

        CreateMap<SafetyStockGridDataModel, SafetyStockGridDataModel>();
    }
}
