using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tamro.Madam.Application.DependencyInjection;
using Tamro.Madam.Application.Models.DependencyInjection;
using Tamro.Madam.Common.SwaggerDoc;
using Tamro.Madam.RestApi.ModelBinding;
using TamroUtilities.Exceptions;
using TamroUtilities.Host.Extensions.Extensions;
using TamroUtilities.Host.Extensions.Models;

namespace Tamro.Madam.RestApi;

public static class ServicesSetup
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddApplication(
            builder.Configuration,
            new ApplicationSettings()
            {
                FeatureFlags = new FeatureFlags
                {
                    AuthorizationByRolesInUserContext = false,
                }
            });

        builder.Services.AddControllers(options =>
            {
                options.ModelBinderProviders.Insert(0, new FilterDefinitionCollectionJsonModelBinderProvider());
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

        builder.AddSwagging();
        builder.Services.AddHealthChecks();
        builder.Services.AddExceptionHandlingFilter();
    }

    private static void AddSwagging(this WebApplicationBuilder builder)
    {
        var additionalSwaggerGenOptionsVisitor = (SwaggerGenOptions opts) =>
        {
            opts.OperationFilter<FilterDefinitionOperationFilter>();
        };

        builder.Services.AddSwaggerTamro(
            "Masterdata service API",
            [FlowType.ClientCredentials, FlowType.Password],
            "/keycloak/connect/token",
            additionalSwaggerGenOptionsVisitor: additionalSwaggerGenOptionsVisitor);
    }
}
