using AutoMapper;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Repository.Entities.Wholesale.Ee;
using Tamro.Madam.Repository.Entities.Wholesale.Lt;
using Tamro.Madam.Repository.Entities.Wholesale.Lv;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Wholesale;

public class ItemAvailabilityProfile : Profile
{
    public ItemAvailabilityProfile()
    {
        CreateMap<LtItemAvailability, ItemAvailabilityModel>()
            .ForMember(d => d.Country, o => o.MapFrom(_ => BalticCountry.LT));

        CreateMap<EeItemAvailability, ItemAvailabilityModel>()
            .ForMember(d => d.Country, o => o.MapFrom(_ => BalticCountry.EE));

        CreateMap<LvItemAvailability, ItemAvailabilityModel>()
            .ForMember(d => d.Country, o => o.MapFrom(_ => BalticCountry.LV));
    }
}
