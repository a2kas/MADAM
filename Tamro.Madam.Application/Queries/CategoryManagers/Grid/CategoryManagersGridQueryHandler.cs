using AutoMapper;
using MediatR;
using System.Linq.Dynamic.Core;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Models.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Queries.CategoryManagers.Grid;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class CategoryManagersGridQueryHandler(
    IMadamUnitOfWork _uow,
    IMapper _mapper)
: IRequestHandler<CategoryManagersGridQuery, PaginatedData<CategoryManagerModel>>
{
    public Task<PaginatedData<CategoryManagerModel>> Handle(CategoryManagersGridQuery request, CancellationToken cancellationToken)
    {
        return _uow.GetRepository<CategoryManager>()
            .AsReadOnlyQueryable()
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
            .ProjectToPaginatedDataAsync<CategoryManager, CategoryManagerModel>(
                request.Specification,
                request.PageNumber,
                request.PageSize,
                _mapper.ConfigurationProvider,
                cancellationToken: cancellationToken);

    }
}
