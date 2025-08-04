using MediatR;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Queries.Items.Bindings;
using Tamro.Madam.Models.Overview;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Application.Handlers.ItemMasterdata.Items.Bindings;

[NoPermissionNeeded]
public class ItemBindingCountByCompanyQueryHandler : IRequestHandler<ItemBindingCountByCompanyQuery, IEnumerable<CompanyBindingCountModel>>
{
    private readonly IItemBindingRepository _itemBindingRepository;

    public ItemBindingCountByCompanyQueryHandler(IItemBindingRepository itemBindingRepository)
    {
        _itemBindingRepository = itemBindingRepository;
    }

    public async Task<IEnumerable<CompanyBindingCountModel>> Handle(ItemBindingCountByCompanyQuery request, CancellationToken cancellationToken)
    {
        return await _itemBindingRepository.GetCountByCompany(cancellationToken);
    }
}
