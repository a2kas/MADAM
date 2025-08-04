using System.Linq.Expressions;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.Bindings.Vlk;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings.Vlk;

public interface IVlkBindingRepository
{
    Task<IEnumerable<VlkBinding>> GetList(Expression<Func<VlkBinding, bool>> filter, List<IncludeOperation<VlkBinding>>? includes = null, bool track = true, CancellationToken cancellationToken = default);
}
