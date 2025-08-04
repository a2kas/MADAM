using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items.Bindings.Retail;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings.Retail;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class GeneratedRetailCodeQueryHandler : IRequestHandler<GeneratedRetailCodeQuery, PaginatedData<GeneratedRetailCodeModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GeneratedRetailCodeQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<GeneratedRetailCodeModel>> Handle(GeneratedRetailCodeQuery request, CancellationToken cancellationToken)
    {
        var query = _uow.GetRepository<GeneratedRetailCode>().AsReadOnlyQueryable();

        if (request.OrderBy == nameof(GeneratedRetailCodeModel.FullCode))
        {
            query = query.OrderBy($"{nameof(GeneratedRetailCodeModel.CodePrefix)} {request.SortDirection}")
                .ThenBy($"{nameof(GeneratedRetailCodeModel.Code)} {request.SortDirection}");
        }
        else
        {
            query = query.OrderBy($"{request.OrderBy} {request.SortDirection}");
        }

        return await query.ProjectToPaginatedDataAsync<GeneratedRetailCode, GeneratedRetailCodeModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
