using AutoMapper;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Retail;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata.Items.Bindings.Retail;

public class GeneratedRetailCodeProfile : Profile
{
    public GeneratedRetailCodeProfile()
    {
        CreateMap<GeneratedRetailCodeModel, GeneratedRetailCode>()
            .ReverseMap();
    }
}
