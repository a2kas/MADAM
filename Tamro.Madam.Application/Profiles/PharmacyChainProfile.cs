using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Profiles
{
    public class PharmacyChainProfile : Profile
    {
        public PharmacyChainProfile()
        {
            CreateMap<SafetyStockPharmacyChain, PharmacyChainModel>()
                .ReverseMap();
        }
    }
}
