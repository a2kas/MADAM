namespace Tamro.Madam.RestApi.ApiRoutePrefix;

public static class AppApiVersioningExtensions
{
    public static void UseApiRoutePrefix(this WebApplication app)
    {
        var pathPrefix = $"/api/masterdata";
        app.UseMiddleware<GlobalRoutePrefixMiddleware>(pathPrefix);
        app.UsePathBase(pathPrefix);
    }
}
