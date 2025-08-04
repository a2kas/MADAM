using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.MeasurementUnits;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UpsertMeasurementUnitCommandHandler : IRequestHandler<UpsertMeasurementUnitCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpsertMeasurementUnitCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpsertMeasurementUnitCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<MeasurementUnit>(request.Model);
        var trackedEntity = await _uow.GetRepository<MeasurementUnit>().UpsertAsync(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(trackedEntity.Id);
    }
}
