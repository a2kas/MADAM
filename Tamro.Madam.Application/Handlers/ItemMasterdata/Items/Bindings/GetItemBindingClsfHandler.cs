using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items.Bindings.Clsf;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
[RequiresPermission(Permissions.CanManageVlkBindings)]
public class GetItemBindingClsfHandler : IRequestHandler<ItemBindingClsfQuery, PaginatedData<ItemBindingClsfModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetItemBindingClsfHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<ItemBindingClsfModel>> Handle(ItemBindingClsfQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<ItemBinding>()
            .AsReadOnlyQueryable()
            .Include(x => x.Item)
            .OrderBy($"{nameof(ItemBinding.LocalId)} asc")
            .ProjectToPaginatedDataAsync<ItemBinding, ItemBindingClsfModel>(request.Specification, 1, 20, _mapper.ConfigurationProvider, cancellationToken);
    }
}
