using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Requestors;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Requestors;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UpsertRequestorCommandHandler : IRequestHandler<UpsertRequestorCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpsertRequestorCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpsertRequestorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Requestor>(request.Model);
        var trackedEntity = await _uow.GetRepository<Requestor>().UpsertAsync(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(trackedEntity.Id);
    }
}
