using MudBlazor;
using Tamro.Madam.Ui.Constants;

namespace Tamro.Madam.Ui.Services.Layout;

public class LayoutService
{
    public MudTheme CurrentTheme { get; private set; } = Theme.GetTamroTheme();
}
