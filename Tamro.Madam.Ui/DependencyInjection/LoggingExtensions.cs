using Serilog;
using System.Reflection;
using TamroUtilities.Logging;

namespace Tamro.Madam.Ui.DependencyInjection;

public static class LoggingExtensions
{
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Services.CreateDefaultElasticConfiguration(builder.Configuration, Assembly.GetEntryAssembly().GetName().Name)
            .CreateHangfireConfiguration()
            .AddLogger();

        builder.Host.UseSerilog();

        return builder;
    }
}
