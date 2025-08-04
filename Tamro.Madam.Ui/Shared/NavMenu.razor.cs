using Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Tamro.Madam.Models.Navigation.Menu;
using Tamro.Madam.Ui.Services.Navigation;
using Tamro.Madam.Ui.Store.State;

namespace Tamro.Madam.Ui.Shared;

public partial class NavMenu
{
    private List<MenuSectionModel> Model { get; set; } = [];

    [Inject]
    protected IMenuService MenuService { get; set; }
    [Inject]
    protected NavigationManager NavigationManager { get; set; }
    [Inject]
    private IState<UserProfileState> _userProfileState { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Model = [];

        foreach (var section in MenuService.Features)
        {
            var newSection = new MenuSectionModel
            {
                Title = section.Title,
                Permissions = section.Permissions,
                SectionItems = []
            };

            if (_userProfileState.Value.UserProfile.Permissions == null)
            {
                return;
            }

            if (section.Permissions == null ||
                section.Permissions.Length == 0 ||
                section.Permissions.Any(p => _userProfileState.Value.UserProfile.Permissions.Contains(p)))
            {
                foreach (var item in section.SectionItems)
                {
                    bool itemIsAllowed = item.Permissions == null ||
                                         item.Permissions.Length == 0 ||
                                         item.Permissions.Any(p => _userProfileState.Value.UserProfile.Permissions.Contains(p));

                    if (itemIsAllowed)
                    {
                        var allowedSubItems = GetAllowedSubItems(item.MenuItems);

                        if (allowedSubItems.Count != 0 || !item.IsParent)
                        {
                            newSection.SectionItems.Add(new MenuSectionItemModel
                            {
                                Title = item.Title,
                                Icon = item.Icon,
                                Href = item.Href,
                                Target = item.Target,
                                Permissions = item.Permissions,
                                PageStatus = item.PageStatus,
                                IsParent = item.IsParent,
                                MenuItems = allowedSubItems
                            });
                        }
                    }
                }

                if (newSection.SectionItems.Any(item => !item.IsParent || item.MenuItems.Count > 0))
                {
                    Model.Add(newSection);
                }
            }
        }
    }

    private List<MenuSectionItemModel> GetAllowedSubItems(List<MenuSectionItemModel> menuItems)
    {
        if (menuItems == null)
        {
            return new List<MenuSectionItemModel>();
        }

        return menuItems
            .Select(subItem => new
            {
                SubItem = subItem,
                IsAllowed = subItem.Permissions == null ||
                            subItem.Permissions.Length == 0 ||
                            subItem.Permissions.Any(p => _userProfileState.Value.UserProfile.Permissions.Contains(p))
            })
            .Where(x => x.IsAllowed)
            .Select(x => new MenuSectionItemModel
            {
                Title = x.SubItem.Title,
                Icon = x.SubItem.Icon,
                Href = x.SubItem.Href,
                Target = x.SubItem.Target,
                Permissions = x.SubItem.Permissions,
                PageStatus = x.SubItem.PageStatus,
                IsParent = x.SubItem.IsParent,
                MenuItems = GetAllowedSubItems(x.SubItem.MenuItems)
            })
            .Where(subItem => subItem.MenuItems == null || subItem.MenuItems.Count > 0 || !subItem.IsParent)
            .ToList();
    }

    private bool Expanded(MenuSectionItemModel menu)
    {
        string href = NavigationManager.Uri[(NavigationManager.BaseUri.Length - 1)..];
        return menu is { IsParent: true, MenuItems: not null } &&
               menu.MenuItems.Any(x => !string.IsNullOrEmpty(x.Href) && x.Href.Equals(href));
    }
}
