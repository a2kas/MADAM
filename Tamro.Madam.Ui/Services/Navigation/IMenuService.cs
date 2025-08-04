using Tamro.Madam.Models.Navigation.Menu;

namespace Tamro.Madam.Ui.Services.Navigation;

public interface IMenuService
{
    IEnumerable<MenuSectionModel> Features { get; }
}
