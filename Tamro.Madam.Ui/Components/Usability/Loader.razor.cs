using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Tamro.Madam.Ui.Components.Usability;

public partial class Loader
{
    [Parameter]
    public Size Size { get; set; } = Size.Large;
}
