using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings;

[RequiresPermission(Permissions.CanViewCoreMasterdata)]
public class GetItemItemBindingsCommandHandler : IRequestHandler<GetItemItemBindingsCommand, Result<IEnumerable<ItemBindingModel>>>
{
    private readonly IItemBindingRepository _repository;
    private readonly IMapper _mapper;

    public GetItemItemBindingsCommandHandler(IItemBindingRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<ItemBindingModel>>> Handle(GetItemItemBindingsCommand request, CancellationToken cancellationToken)
    {
        var includes = new List<IncludeOperation<ItemBinding>>
        {
            new(q => q.Include(ib => ib.Languages)),
        };
            
        var bindings = await _repository.GetList(x => x.ItemId == request.ItemId, includes, track: false, cancellationToken: cancellationToken);
        return Result<IEnumerable<ItemBindingModel>>.Success(_mapper.Map<IEnumerable<ItemBindingModel>>(bindings));
    }
}
