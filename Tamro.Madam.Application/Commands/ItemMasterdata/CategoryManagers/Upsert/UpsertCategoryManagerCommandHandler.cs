using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Upsert;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UpsertCategoryManagerCommandHandler : IRequestHandler<UpsertCategoryManagerCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpsertCategoryManagerCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpsertCategoryManagerCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<CategoryManager>(request);
        var trackedEntity = await _uow.GetRepository<CategoryManager>().UpsertAsync(entity);
        await _uow.SaveChangesAsync(cancellationToken);
        return Result<int>.Success(trackedEntity.Id);
    }
}
