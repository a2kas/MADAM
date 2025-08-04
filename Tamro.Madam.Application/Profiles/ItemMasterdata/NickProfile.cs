using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Nicks;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata;

public class NickProfile : Profile
{
    public NickProfile()
    {
        CreateMap<NickModel, Nick>()
            .ForMember(d => d.Items, o => o.Ignore())
            .ReverseMap();

        CreateMap<Nick, NickClsfModel>();
        CreateMap<NickModel, NickClsfModel>();
    }
}
