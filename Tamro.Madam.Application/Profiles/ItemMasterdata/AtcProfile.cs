using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Atcs;
using Tamro.Madam.Repository.Entities.Atcs;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata;

public class AtcProfile : Profile
{
    public AtcProfile()
    {
        CreateMap<AtcModel, Atc>()
            .ForMember(d => d.Items, o => o.Ignore())
            .ReverseMap();

        CreateMap<Atc, AtcClsfModel>();
    }
}
