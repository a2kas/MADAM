using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Forms;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata;

public class FormProfile : Profile
{
    public FormProfile()
    {
        CreateMap<FormModel, Form>()
            .ForMember(d => d.Items, o => o.Ignore())
            .ReverseMap();

        CreateMap<Form, FormClsfModel>();
    }
}
