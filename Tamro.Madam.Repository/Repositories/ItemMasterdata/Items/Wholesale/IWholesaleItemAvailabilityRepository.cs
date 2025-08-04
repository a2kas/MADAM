using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

public interface IWholesaleItemAvailabilityRepository
{
    Task<IEnumerable<ItemAvailabilityModel>> GetAll();
    Task<IEnumerable<ItemAvailabilityModel>> Get(List<string> itemNo2s);
}
