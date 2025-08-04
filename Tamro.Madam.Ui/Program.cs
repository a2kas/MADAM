using System.Globalization;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Tamro.Madam.Application.DependencyInjection;
using Tamro.Madam.Application.Handlers.Suppliers.Contracts;
using Tamro.Madam.Application.Models.DependencyInjection;
using Tamro.Madam.Ui.DependencyInjection;
using Tamro.Madam.Ui.Utils;
using TamroUtilities.Keycloak.Authentication;

var builder = WebApplication.CreateBuilder(args);

var cultureInfo = new CultureInfo("en-GB");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

builder.Services.AddHangfireServerTamro(new SqlServerStorage(builder.Configuration["Databases:Hangfire"]));
builder.Services.AddServerUI();
builder.Services.AddSecurity();

builder.Services.AddSettings(builder.Configuration);

builder.Services.AddAuthenticationTamro(builder.Configuration);

builder.Services.AddApplication(
    builder.Configuration,
    new ApplicationSettings()
    {
        FeatureFlags = new FeatureFlags
        {
            AuthorizationByRolesInUserContext = true,
        }
    });

builder.Services.AddJobs();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserSettingsUtils>();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();
builder.Services.AddHangfireRequiringJobs();
builder.Services.AddScoped<UploadContractFileCommandHandler>();
builder.AddLogging();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapAuthenticationEndpoints(builder.Configuration);

app.Run();