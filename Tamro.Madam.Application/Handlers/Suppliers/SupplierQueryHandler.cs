using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Suppliers;
using Tamro.Madam.Models.Suppliers;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.Suppliers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.Suppliers;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class SupplierQueryHandler : IRequestHandler<SupplierQuery, PaginatedData<SupplierGridModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public SupplierQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<SupplierGridModel>> Handle(SupplierQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Supplier>().AsReadOnlyQueryable().OrderBy($"{request.OrderBy} {request.SortDirection}")
                    .ProjectToPaginatedDataAsync<Supplier, SupplierGridModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
