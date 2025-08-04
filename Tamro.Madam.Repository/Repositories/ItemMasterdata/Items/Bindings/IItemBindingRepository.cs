using System.Linq.Expressions;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels.Import;
using Tamro.Madam.Models.Overview;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings;

public interface IItemBindingRepository
{
    Task<IEnumerable<ItemBinding>> GetList(Expression<Func<ItemBinding, bool>> filter, List<IncludeOperation<ItemBinding>>? includes = null, bool track = true, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyBindingCountModel>> GetCountByCompany(CancellationToken cancellationToken = default);
    Task<IEnumerable<ItemAssortmentItemModel>> GetSalesChannelItemAssortmentOverview(IEnumerable<string> itemNumbers, IEnumerable<string> companies, CancellationToken cancellationToken);
}
