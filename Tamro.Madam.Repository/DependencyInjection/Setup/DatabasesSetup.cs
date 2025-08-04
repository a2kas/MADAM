using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tamro.Madam.Models.Configuration;
using Tamro.Madam.Repository.Context.E1Gateway;
using Tamro.Madam.Repository.Context.Factories;
using Tamro.Madam.Repository.Context.Jpg;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.Context.Sks;
using Tamro.Madam.Repository.Context.Wholesale;
using Tamro.Madam.Repository.Repositories.Administration.Configuration.Ubl;
using Tamro.Madam.Repository.Repositories.Audit;
using Tamro.Madam.Repository.Repositories.Customers;
using Tamro.Madam.Repository.Repositories.Customers.Notifications;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Ee;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lt;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lv;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.SafetyStocks.Lt;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Ee;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lt;
using Tamro.Madam.Repository.Repositories.ItemMasterdata.Items.Wholesale.Lv;
using Tamro.Madam.Repository.Repositories.Sales;
using Tamro.Madam.Repository.Repositories.Sales.HeldOrders;
using Tamro.Madam.Repository.UnitOfWork;
using TamroUtilities.EFCore.UnitOfWork;

namespace Tamro.Madam.Repository.DependencyInjection.Setup;
internal static class DatabasesSetup
{
    public static IServiceCollection AddAllDatabases(IServiceCollection services, IConfiguration configuration)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning)
                   .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
        });

        services.AddDbContext<MadamDbContext>((provider, optionsBuilder) =>
        {
            var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            optionsBuilder.UseSqlServer(
                databaseSettings.Madam,
                // TODO:
                // https://jira.tamro.lt/browse/OTHERS-1746
                // EF Core 8 has breaking changes: https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/breaking-changes
                // After Sql server upgrate remove changing compability level
                sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(110)
            )
            .UseLoggerFactory(loggerFactory);
        }, ServiceLifetime.Transient);

        services.AddDbContexts(loggerFactory);
        services.AddUnitOfWork();

        services.AddTransient<IDbContextFactory<MadamDbContext>, PooledDbContextFactory<MadamDbContext>>();
        services.AddTransient<IMadamDbContext>(provider => provider.GetRequiredService<IDbContextFactory<MadamDbContext>>().CreateDbContext());

        services.AddTransient<IItemRepository, ItemRepository>();
        services.AddTransient<IItemBindingRepository, ItemBindingRepository>();
        services.AddTransient<IAuditRepository, AuditRepository>();
        services.AddTransient<IBarcodeRepository, BarcodeRepository>();
        services.AddTransient<IGeneratedRetailCodeRepository, GeneratedRetailCodeRepository>();
        services.AddTransient<ISafetyStockConditionRepository, SafetyStockConditionRepository>();
        services.AddTransient<ISafetyStockPharmacyChainRepository, SafetyStockPharmacyChainRepository>();
        services.AddTransient<ISafetyStockRepository, SafetyStockRepository>();
        services.AddTransient<ISafetyStockItemRepository, SafetyStockItemRepository>();
        services.AddTransient<ICustomerLegalEntityRepository, CustomerLegalEntityRepository>();
        services.AddTransient<ICustomerLegalEntityNotificationRepository, CustomerLegalEntityNotificationRepository>();
        services.AddTransient<LtWholesaleItemRepository>();
        services.AddTransient<LvWholesaleItemRepository>();
        services.AddTransient<EeWholesaleItemRepository>();
        services.AddTransient<LvWholesaleCustomerRepository>();
        services.AddTransient<LtWholesaleCustomerRepository>();
        services.AddTransient<EeWholesaleCustomerRepository>();
        services.AddTransient<IE1CanceledOrderRepository, E1CanceledOrderRepository>();
        services.AddTransient<ICanceledLineStatisticRepository, CanceledLineStatisticRepository>();
        services.AddTransient<ICustomerLegalEntityRepository, CustomerLegalEntityRepository>();
        services.AddTransient<IE1HeldOrderRepository, E1HeldOrderRepository>();
        services.AddTransient<IUblApiKeyRepository, UblApiKeyRepository>();

        services.AddDbContext<WhRawLtDatabaseContext>((provider, optionsBuilder) =>
        {
            var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            optionsBuilder.UseSqlServer(
                databaseSettings.WhRawLt,
                sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(110)
            );
        });
        services.AddScoped<IDbContextFactory<WhRawLtDatabaseContext>, PooledDbContextFactory<WhRawLtDatabaseContext>>();
        services.AddTransient<IWhRawLtDatabaseContext>(provider => provider.GetRequiredService<IDbContextFactory<WhRawLtDatabaseContext>>().CreateDbContext());

        services.AddTransient<LtWholesaleItemAvailabilityRepository>();
        services.AddTransient<LtSafetyStockWholesaleRepository>();

        services.AddDbContextPool<IWhRawLvDatabaseContext, WhRawLvDatabaseContext>
            (options => options.UseSqlServer(
                configuration.GetSection("Databases:WhRawLv").Value,
                sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(110))
            );
        services.AddTransient<LvWholesaleItemAvailabilityRepository>();

        services.AddDbContextPool<IWhRawEeDatabaseContext, WhRawEeDatabaseContext>
            (options => options.UseSqlServer(
                configuration.GetSection("Databases:WhRawEe").Value,
                sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(110))
            );
        services.AddTransient<EeWholesaleItemAvailabilityRepository>();

        services.AddDbContextPool<ISksDbContext, SksDbContext>
            (options => options.UseSqlServer(
                configuration.GetSection("Databases:SKSDBConnectionString").Value,
                sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(100))
            );

        services.AddDbContext<E1GatewayDbContext>((provider, optionsBuilder) =>
        {
            var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            optionsBuilder.UseSqlServer(
                databaseSettings.E1Gateway,
                sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(130)
            );
        });
        services.AddScoped<IDbContextFactory<E1GatewayDbContext>, PooledDbContextFactory<E1GatewayDbContext>>();
        services.AddTransient<IE1GatewayDbContext>(provider => provider.GetRequiredService<IDbContextFactory<E1GatewayDbContext>>().CreateDbContext());
        services.AddTransient<TamroUtilities.EFCore.UnitOfWork.IUnitOfWork<E1GatewayDbContext>, UnitOfWork<E1GatewayDbContext>>();

        services.AddDbContext<E1DbContext>((provider, optionsBuilder) =>
        {
            var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            optionsBuilder.UseSqlServer(
                databaseSettings.E1,
                sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(110)
            );
        });
        services.AddScoped<IDbContextFactory<E1DbContext>, PooledDbContextFactory<E1DbContext>>();
        services.AddTransient<IE1DbContext>(provider => provider.GetRequiredService<IDbContextFactory<E1DbContext>>().CreateDbContext());

        return services;
    }

    private static void AddDbContexts(this IServiceCollection services, ILoggerFactory loggerFactory)
    {
        services.AddDbContext<Tamro.Madam.Repository.Context.Madam.MadamDbContext>((provider, optionsBuilder) =>
        {
            var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            optionsBuilder.UseSqlServer(
                databaseSettings.Madam,
                // TODO:
                // https://jira.tamro.lt/browse/OTHERS-1746
                // EF Core 8 has breaking changes: https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/breaking-changes
                // After Sql server upgrate remove changing compability level
                sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(110)
            )
            .UseLoggerFactory(loggerFactory);
        }, ServiceLifetime.Transient);

        services.AddDbContext<JpgDbContext>((provider, optionsBuilder) =>
        {
            var databaseSettings = provider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            optionsBuilder.UseSqlServer(
                databaseSettings.Jira,
                // TODO:
                // https://jira.tamro.lt/browse/OTHERS-1746
                // EF Core 8 has breaking changes: https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-8.0/breaking-changes
                // After Sql server upgrate remove changing compability level
                sqlServerOptions => sqlServerOptions.UseCompatibilityLevel(140)
            )
            .UseLoggerFactory(loggerFactory);
        });
    }

    private static void AddUnitOfWork(this IServiceCollection services)
    {
        services.AddTransient<IMadamUnitOfWork>(provider => new MadamUnitOfWork(
                provider.GetRequiredService<MadamDbContext>())
            );
        services.AddTransient<IJpgUnitOfWork>(provider => new JpgUnitOfWork(
            provider.GetRequiredService<JpgDbContext>())
        );
    }
}
