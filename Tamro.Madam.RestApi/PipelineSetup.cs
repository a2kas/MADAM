using Tamro.Madam.RestApi.ApiRoutePrefix;
using TamroUtilities.Exceptions;
using TamroUtilities.Host.Extensions.Extensions;

namespace Tamro.Madam.RestApi;

public static class PipelineSetup
{
    public static void SetupPipeline(this WebApplication app)
    {
        app.UseMiddleware(typeof(ServiceErrorHandlingMiddleware));

        app.UseHttpsRedirection();

        app.UseApiRoutePrefix();

        app.UseSwaggerTamro("Masterdata Service API");

        app.UseRouting();

        app.UseHealthChecks("/health");

        app.UseAuthorization(); // to be replaced - see TransactionService

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });

    }
}
