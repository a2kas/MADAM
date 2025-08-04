using Tamro.Madam.Ui.DependencyInjection;

namespace Tamro.Madam.Ui.Pages.Dev.Samples.AppVersion;

public partial class EnvironmentVariables
{
    private List<(string Key, string Value)> environmentVariables = new List<(string Key, string Value)>
    {
        ("APP_VERSION", EnvironmentalVariables.AppVersion),
        ("ASPNETCORE_ENVIRONMENT", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Not available")
    };
}
