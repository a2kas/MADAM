using AutoMapper;
using Tamro.Madam.Application.Queries.CategoryManagers.Grid;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.CategoryManagers.Index;

namespace Tamro.Madam.RestApi.Profiles.ItemMasterdata.CategoryManagers;

public class CategoryManagerProfile : Profile
{
    public CategoryManagerProfile()
    {
        CreateMap<CategoryManagerSearchViewModel, CategoryManagersGridQuery>()
            .ForMember(cm => cm.Specification, o => o.Ignore())
            .ForMember(cm => cm.ErrorMessage, o => o.Ignore());

        CreateMap<CategoryManagerModel, CategoryManagerViewModel>();
        CreateMap<PaginatedData<CategoryManagerModel>, PaginatedData<CategoryManagerViewModel>>();
    }
}
