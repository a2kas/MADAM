using Fluxor;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Ui.Store.State;
using Tamro.Madam.Ui.Store.State.ItemMasterdata;

namespace Tamro.Madam.Ui.Pages.ItemMasterdata.Items.Components.Tabs;

public partial class ItemAdditinalInfoTab
{
    [Inject]
    private IState<ItemState> _itemState { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }

    private List<ItemType> _itemTypes = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().OrderBy(item => item.ToString()).ToList();
}