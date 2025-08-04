using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items.Bindings.Vlk;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings.Vlk;

[RequiresPermission(Permissions.CanManageVlkBindings)]
public class VlkBindingQueryHandler : IRequestHandler<VlkBindingQuery, PaginatedData<VlkBindingGridModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public VlkBindingQueryHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<VlkBindingGridModel>> Handle(VlkBindingQuery request, CancellationToken cancellationToken)
    {
        var query = _uow.GetRepository<VlkBinding>()
            .AsReadOnlyQueryable()
            .Include(x => x.ItemBinding)
            .ThenInclude(x => x.Item);

        return await query.ProjectToPaginatedDataAsync<VlkBinding, VlkBindingGridModel>(request.Specification, request.PageNumber, request.PageSize, _mapper.ConfigurationProvider, cancellationToken);
   }
}
