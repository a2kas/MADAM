using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Requestors;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata;

public class RequestorProfile : Profile
{
    public RequestorProfile()
    {
        CreateMap<RequestorModel, Requestor>()
           .ForMember(d => d.Items, o => o.Ignore())
           .ReverseMap();
        CreateMap<RequestorModel, RequestorClsfModel>();
        CreateMap<Requestor, RequestorClsfModel>();
    }
}
