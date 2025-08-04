using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Application.Jobs.Hangfire;
using TamroUtilities.Hangfire.Helpers;

namespace Tamro.Madam.Application.DependencyInjection;

public static class HangfireRequiringJob
{
    public static void AddHangfireRequiringJobs(this IServiceCollection services)
    {
        services.AddTransientRequiringJob<SafetyStockWhslQtyUpdateBase, SafetyStockWhslQtyUpdate>(TamroCronExpressions.MinutesIntervalFromTill(10, 7, 22));
        services.AddTransientRequiringJob<SafetyStockItemsUpdateBase, SafetyStockItemsUpdate>(TamroCronExpressions.MinutesIntervalFromTill(59, 5, 13));
        services.AddTransientRequiringJob<SendCanceledLineEmailsBase, SendCanceledLineEmails>(TamroCronExpressions.MinutesIntervalFromTill(30, 6, 21));
        services.AddTransientRequiringJob<SendHeldOrderEmailsBase, SendHeldOrderEmails>(TamroCronExpressions.MinutesIntervalFromTill(20, 6, 21));
    }
}
