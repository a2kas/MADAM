using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Producers;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata;

public class ProducerProfile : Profile
{
    public ProducerProfile()
    {
        CreateMap<ProducerModel, Producer>()
            .ForMember(d => d.Items, o => o.Ignore())
            .ReverseMap();

        CreateMap<Producer, ProducerClsfModel>();
        CreateMap<ProducerModel, ProducerClsfModel>();
    }
}
