using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items.SafetyStocks;

public class SafetyStockConditionProfile : Profile
{
    public SafetyStockConditionProfile()
    {
        CreateMap<SafetyStockCondition, SafetyStockConditionModel>()
            .ForMember(d => d.CheckDays, o => o.MapFrom(src => src.SafetyStockItem.CheckDays));
        CreateMap<SafetyStockConditionModel, SafetyStockCondition>()
            .ForMember(d => d.SafetyStockItem, o => o.Ignore())
            .ForMember(d => d.SafetyStockPharmacyChain, o => o.Ignore());
        CreateMap<SafetyStockConditionUpsertModel, SafetyStockConditionModel>()
            .ForMember(d => d.RestrictionLevel, o => o.MapFrom(src => src.PharmacyChainId == default ? SafetyStockRestrictionLevel.PharmacyChainGroup : SafetyStockRestrictionLevel.PharmacyChain))
            .ForMember(d => d.SafetyStockPharmacyChainId, o => o.MapFrom(src => src.PharmacyChainId))
            .ForMember(d => d.SafetyStockPharmacyChainGroup, o => o.MapFrom(src => src.PharmacyGroup))
            .ForMember(d => d.SafetyStockItemId, o => o.Ignore())
            .ForMember(d => d.User, o => o.Ignore())
            .ForMember(d => d.CreatedDate, o => o.Ignore())
            .ForMember(d => d.RowVer, o => o.Ignore());
    }
}
