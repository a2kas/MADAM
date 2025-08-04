using Microsoft.AspNetCore.Components;

namespace Tamro.Madam.Ui.Pages.Dev.Samples.Elk;

public partial class Elk
{
    [Inject]
    private ILogger<Elk> _logger { get; set; }

    private async Task OnLogHandledError()
    {
        _logger.LogError("Handled error from MADAM");
    }

    private async Task OnLogUnhandledError()
    {
        throw new NotImplementedException();
    }

    private async Task OnLogWarning()
    {
        _logger.LogWarning("Warning from MADAM");
    }

    private async Task OnLogInfo()
    {
        _logger.LogInformation("Info from MADAM");
    }
}
