using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.MeasurementUnits;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.MeasurementUnits;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class MeasurementUnitQueryHandler : IRequestHandler<MeasurementUnitQuery, PaginatedData<MeasurementUnitModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MeasurementUnitQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<MeasurementUnitModel>> Handle(MeasurementUnitQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<MeasurementUnit>().AsReadOnlyQueryable().OrderBy($"{request.OrderBy} {request.SortDirection}")
                    .ProjectToPaginatedDataAsync<MeasurementUnit, MeasurementUnitModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
    }
}
