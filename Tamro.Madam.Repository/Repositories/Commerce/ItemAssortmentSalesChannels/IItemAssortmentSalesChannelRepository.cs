using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;

namespace Tamro.Madam.Repository.Repositories.Commerce.ItemAssortmentSalesChannels;

public interface IItemAssortmentSalesChannelRepository
{
    Task<ItemAssortmentSalesChannelDetailsModel> UpsertGraph(ItemAssortmentSalesChannelDetailsModel model);
    Task<ItemAssortmentSalesChannelDetailsModel> Get(int id);
}
