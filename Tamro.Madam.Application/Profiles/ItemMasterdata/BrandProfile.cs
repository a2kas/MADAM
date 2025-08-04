using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Repository.Entities.Brands;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<BrandModel, Brand>()
            .ForMember(d => d.Items, o => o.Ignore())
            .ReverseMap();

        CreateMap<Brand, BrandClsfModel>();
        CreateMap<BrandModel, BrandClsfModel>();
    }
}
