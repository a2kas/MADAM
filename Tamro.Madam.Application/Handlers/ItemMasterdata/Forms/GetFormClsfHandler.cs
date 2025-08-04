using System.Linq.Dynamic.Core;
using AutoMapper;
using MediatR;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Extensions;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Forms.Clsf;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Forms;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Forms;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetFormClsfHandler : IRequestHandler<FormClsfQuery, PaginatedData<FormClsfModel>>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetFormClsfHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<PaginatedData<FormClsfModel>> Handle(FormClsfQuery request, CancellationToken cancellationToken)
    {
        return await _uow.GetRepository<Form>().AsReadOnlyQueryable().OrderBy($"{nameof(Form.Name)} asc")
                    .ProjectToPaginatedDataAsync<Form, FormClsfModel>(request.Specification, 1, 20, _mapper.ConfigurationProvider, cancellationToken);
    }
}
