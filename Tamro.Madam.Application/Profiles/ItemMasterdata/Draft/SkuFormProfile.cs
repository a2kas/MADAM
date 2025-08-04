using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.SkuForms;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Draft;
public class SkuFormProfile : Profile
{
    public SkuFormProfile()
    {
        CreateMap<SkuForm, SkuFormModel>()
            .ForMember(m => m.FullVersion, o => o.Ignore())
            .ForMember(m => m.Name, o => o.Ignore());
    }
}
