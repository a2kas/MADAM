namespace Tamro.Madam.Ui.DependencyInjection;

public static class EnvironmentalVariables
{
    public static string AppVersion { get; } = Environment.GetEnvironmentVariable("APP_VERSION") ?? "0";
}
