using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

public interface IWholesaleItemRepository
{
    Task<PaginatedData<WholesaleItemClsfModel>> GetClsf(string searchTerm, int pageNumber, int pageSize);
    Task<PaginatedData<WholesaleItemClsfModel>> GetClsf(List<string> itemNo2s, int pageNumber, int pageSize);
    Task<List<WholesaleItemModel>> GetMany(WholesaleItemSearchModel search);
}
