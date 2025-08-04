using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale;

namespace Tamro.Madam.Application.Services.Items.Wholesale.Factories;

public interface IWholesaleItemRepositoryFactory
{
    IWholesaleItemRepository Get(BalticCountry country);
}
