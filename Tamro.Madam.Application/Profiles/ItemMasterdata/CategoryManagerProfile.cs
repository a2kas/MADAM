using AutoMapper;
using Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Upsert;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;

namespace Tamro.Madam.Application.Profiles.ItemMasterdata;
public class CategoryManagerProfile : Profile
{
    public CategoryManagerProfile()
    {
        CreateMap<CategoryManager, CategoryManagerModel>();

        CreateMap<CategoryManagerModel, UpsertCategoryManagerCommand>()
            .ForMember(command => command.EmailAddress, o => o.MapFrom(m => m.EmailAddress.Trim()))
            .ForMember(command => command.FirstName, o => o.MapFrom(m => m.FirstName.Trim()))
            .ForMember(command => command.LastName, o => o.MapFrom(m => m.LastName.Trim()));

        CreateMap<UpsertCategoryManagerCommand, CategoryManager>()
            .ForMember(cm => cm.CreatedDate, o => o.Ignore())
            .ForMember(cm => cm.NewProductOffers, o => o.Ignore());
    }
}
