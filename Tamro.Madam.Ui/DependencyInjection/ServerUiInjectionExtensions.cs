using BlazorDownloadFile;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using MudBlazor;
using MudBlazor.Services;
using MudExtensions.Services;
using System.Reflection;
using Tamro.Madam.Ui.Services.Layout;
using Tamro.Madam.Ui.Services.Navigation;
using Tamro.Madam.Ui.Services.Storage;

namespace Tamro.Madam.Ui.DependencyInjection;

public static class ServerUiInjectionExtensions
{
    public static IServiceCollection AddServerUI(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor(options =>
        {
            options.DetailedErrors = true;
            options.DisconnectedCircuitMaxRetained = 100;
            options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
            options.MaxBufferedUnacknowledgedRenderBatches = 10;
        }).AddHubOptions(options =>
        {
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
            options.EnableDetailedErrors = false;
            options.HandshakeTimeout = TimeSpan.FromSeconds(15);
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
            options.MaximumParallelInvocationsPerClient = 100;
            options.MaximumReceiveMessageSize = 1024 * 1024;
            options.StreamBufferCapacity = 10;
        }).AddCircuitOptions(option => { option.DetailedErrors = true; });

        services.AddFluxor(options =>
        {
            options.ScanAssemblies(Assembly.GetExecutingAssembly());
            options.UseReduxDevTools();
        });

        services.AddMudBlazorDialog()
            .AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.NewestOnTop = true;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 8000;
                config.SnackbarConfiguration.HideTransitionDuration = 500;
                config.SnackbarConfiguration.ShowTransitionDuration = 500;
                config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
                config.PopoverOptions.Mode = PopoverMode.Default;
            });

        services.AddMudExtensions();
        services.AddBlazorDownloadFile();
        services.AddScoped<LayoutService>();
        services.AddTransient<IMenuService, MenuService>();
        services.AddScoped<ILocalStorageService, LocalStorageService>();

        return services;
    }
}
