namespace Tamro.Madam.Models.Navigation.Menu;

public class MenuSectionModel
{
    public string Title { get; set; } = string.Empty;
    public string[]? Permissions { get; set; }
    public List<MenuSectionItemModel>? SectionItems { get; set; }
}
