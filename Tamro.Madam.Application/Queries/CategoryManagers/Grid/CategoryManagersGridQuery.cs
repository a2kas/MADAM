using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Queries.Items;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.CategoryManagers.Grid;
public class CategoryManagersGridQuery : CategoryManagerFilter, IRequest<PaginatedData<CategoryManagerModel>>, IDefaultErrorMessage
{
    public CategoryManagerSpecification Specification => new(this);

    public string ErrorMessage { get; set; } = "Failed to fetch data for category managers grid";
}
