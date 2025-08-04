using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Brands;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.Brands;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Brands;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UpsertBrandCommandHandler : IRequestHandler<UpsertBrandCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpsertBrandCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpsertBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Brand>(request.Model);
        var trackedEntity = await _uow.GetRepository<Brand>().UpsertAsync(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(trackedEntity.Id);
    }
}
