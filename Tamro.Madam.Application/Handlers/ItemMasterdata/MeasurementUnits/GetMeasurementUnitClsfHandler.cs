using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.MeasurementUnits.Clsf;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.MeasurementUnits;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetMeasurementUnitClsfHandler : IRequestHandler<MeasurementUnitClsfQuery, PaginatedData<MeasurementUnitClsfModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetMeasurementUnitClsfHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<MeasurementUnitClsfModel>> Handle(MeasurementUnitClsfQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<MeasurementUnit>().AsReadOnlyQueryable().OrderBy($"{nameof(MeasurementUnit.Name)} asc")
                    .ProjectToPaginatedDataAsync<MeasurementUnit, MeasurementUnitClsfModel>(request.Specification, 1, 20, _mapper.ConfigurationProvider, cancellationToken);
    }
}
