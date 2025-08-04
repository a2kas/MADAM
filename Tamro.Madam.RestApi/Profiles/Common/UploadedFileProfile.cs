using AutoMapper;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.RestApi.Profiles.Common;

public class UploadedFileProfile : Profile
{
    public UploadedFileProfile()
    {
        CreateMap<IFormFile, FileWithName>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FileName))
            .ForMember(dest => dest.Stream, opt => opt.MapFrom(src => src.OpenReadStream()));
    }
}
