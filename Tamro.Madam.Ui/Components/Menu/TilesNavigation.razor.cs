using Fluxor;
using Microsoft.AspNetCore.Components;
using Tamro.Madam.Models.Navigation.Menu;
using Tamro.Madam.Ui.Services.Navigation;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Components.Menu;

public partial class TilesNavigation
{
    private List<MenuSectionItemModel> Model { get; set; } = [];

    [Inject]
    protected IMenuService MenuService { get; set; }

    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Model = [];

        foreach (var section in MenuService.Features)
        {
            var allowedItems = GetAllowedSubItems(section.SectionItems);
            Model.AddRange(allowedItems);
        }
    }

    private List<MenuSectionItemModel> GetAllowedSubItems(List<MenuSectionItemModel>? menuItems)
    {
        if (menuItems == null)
        {
            return new List<MenuSectionItemModel>();
        }

        var allowedItems = new List<MenuSectionItemModel>();

        foreach (var subItem in menuItems)
        {
            if ((subItem.Permissions == null || subItem.Permissions.Length == 0 ||
                subItem.Permissions.Any(p => _userProfileState.Value.UserProfile.Permissions.Contains(p))) &&
                !string.IsNullOrEmpty(subItem.Href))
            {
                allowedItems.Add(new MenuSectionItemModel
                {
                    Title = subItem.Title,
                    Icon = subItem.Icon,
                    Href = subItem.Href,
                    Target = subItem.Target,
                    Permissions = subItem.Permissions,
                    PageStatus = subItem.PageStatus,
                    IsParent = subItem.IsParent,
                    MenuItems = GetAllowedSubItems(subItem.MenuItems)
                });
            }

            if (subItem.MenuItems != null && subItem.MenuItems.Count > 0)
            {
                allowedItems.AddRange(GetAllowedSubItems(subItem.MenuItems));
            }
        }

        return allowedItems;
    }
}
