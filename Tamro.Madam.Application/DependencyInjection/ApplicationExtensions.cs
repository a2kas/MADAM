using AutoMapper;
using AutoMapper.EquivalencyExpression;
using AutoMapper.Extensions.ExpressionMapping;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using System.Reflection;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Infrastructure.Cache;
using Tamro.Madam.Application.Infrastructure.Session;
using Tamro.Madam.Application.Jobs;
using Tamro.Madam.Application.Jobs.Hangfire;
using Tamro.Madam.Application.Models.DependencyInjection;
using Tamro.Madam.Application.Services.Authentication;
using Tamro.Madam.Application.Services.Customers.Factories;
using Tamro.Madam.Application.Services.Employees.Factories;
using Tamro.Madam.Application.Services.Files;
using Tamro.Madam.Application.Services.Items.QualityCheck;
using Tamro.Madam.Application.Services.Items.SafetyStocks;
using Tamro.Madam.Application.Services.Items.SafetyStocks.Factory;
using Tamro.Madam.Application.Services.Items.Wholesale.Factories;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Decorators;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Factories;
using Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Resolvers;
using Tamro.Madam.Application.Services.Sales.Decorators;
using Tamro.Madam.Application.Services.Sales.HeldOrders;
using Tamro.Madam.Application.Services.Sales.HeldOrders.Factories;
using Tamro.Madam.Application.Services.Sales.HeldOrders.Resolvers;
using Tamro.Madam.Application.Services.Settings;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Common.Configuration;
using Tamro.Madam.Repository.Context.Madam;
using Tamro.Madam.Repository.DependencyInjection;
using Tamro.Madam.Repository.Repositories.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Repositories.Customers.E1;
using Tamro.Madam.Repository.Repositories.Customers.Wholesale.Lv;
using Tamro.Madam.Repository.Repositories.Sales.HeldOrders;
using Tamro.Madam.Repository.Repositories.Suppliers;
using Tamro.Madam.Ui.DependencyInjection;
using Tamroutilities.Email.Sender;
using TamroUtilities.MinIO;


namespace Tamro.Madam.Application.DependencyInjection;

public static class ApplicationExtensions
{
    /// <summary>
    /// Expects the following Services to be already registered: <br/>
    /// - <see cref="TamroUtilities.MinIO.IFileStorage"/> <br/>
    /// Also adds the Repository layer
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration configuration,
        ApplicationSettings applicationServicesSettings)
    {
        services.AddConfiguration(configuration);

        services.AddRepository(configuration);

        return services.AddServices(configuration, applicationServicesSettings.FeatureFlags);
    }

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<MinioSettings>(configuration.GetSection("Minio"));
    }

    public static IServiceCollection AddServices(
        this IServiceCollection services,
        IConfiguration configuration,
        FeatureFlags featureFlags)
    {
        services.AddScoped<IUserContext, UserContext>();

        services.AddScoped<IFileStorage, MinioFileStorage>();

        services.AddInternalHttpClients(configuration);

        services.AddGptService(configuration);

        services.AddAutoMapper((serviceProvider, automapper) =>
            {
                automapper.AddCollectionMappers();
                automapper.UseEntityFrameworkCoreModel<MadamDbContext>(serviceProvider);
                automapper.AddExpressionMapping();
            },
            Assembly.GetExecutingAssembly(),
            Assembly.GetEntryAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
        {
            return memberInfo.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>()?.DisplayName;
        };
        Assembly[] assembliesToScan = { Assembly.GetExecutingAssembly() };
        MediatRHandlerCache.PopulateHandlerCache(assembliesToScan);

        AddMediatrPipeline(services, featureFlags.AuthorizationByRolesInUserContext);

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IValidationService, ValidationService>();
        services.AddTransient<IHandlerValidator, HandlerValidator>();
        services.AddTransient<IExcelService, ExcelService>();
        services.AddTransient<IUserSettingsService, UserSettingsService>();
        services.AddTransient<IPermissionService, PermissionService>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<ICanceledOrderLinesEmailGeneratorFactory, CanceledOrderLinesEmailGeneratorFactory>();
        services.AddTransient<LvCanceledOrderLinesEmailGenerator>();
        services.AddTransient<LtCanceledOrderLinesEmailGenerator>();
        services.AddTransient<EeCanceledOrderLinesEmailGenerator>();
        services.AddTransient<ISalesOrderCustomerDecorator, SalesOrderCustomerDecorator>();
        services.AddTransient<ICanceledOrderLineItemDecorator, CanceledOrderLineItemDecorator>();
        services.AddItemServices();
        services.AddSupplierServices();
        services.AddCustomerServices();
        services.AddCanceledOrderServices();
        services.AddHeldOrderServices();

        services.AddMinio(configureClient => configureClient
            .WithEndpoint(configuration.GetSection("Minio:Endpoint").Value)
            .WithCredentials(configuration.GetSection("Minio:AccessKey").Value, configuration.GetSection("Minio:SecretKey").Value)
            .Build());
        return services;
    }

    private static void AddMediatrPipeline(
        IServiceCollection services,
        bool authorizationByRolesFromUserContext)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));

        if (authorizationByRolesFromUserContext)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        }
    }

    public static IServiceCollection AddJobs(this IServiceCollection services)
    {
        services.AddTransient<ItemLogDataMigrationJob>();
        services.AddTransient<ItemMasterdataQualityCheckJob>();
        services.AddTransient<SafetyStockWhslQtyUpdate>();
        services.AddTransient<SafetyStockItemsUpdate>();
        services.AddTransient<SendCanceledLineEmails>();
        services.AddTransient<SendHeldOrderEmails>();

        return services;
    }

    private static IServiceCollection AddItemServices(this IServiceCollection services)
    {
        services.AddTransient<ISafetyStockService, SafetyStockService>();
        services.AddTransient<ISafetyStockItemsUpdateService, LtSafetyStockItemsUpdateService>();
        services.AddTransient<ISafetyStockWholesaleRepositoryFactory, SafetyStockWholesaleRepositoryFactory>();
        services.AddTransient<IWholesaleItemRepositoryFactory, WholesaleItemRepositoryFactory>();
        services.AddTransient<IWholesaleItemAvailabilityRepositoryFactory, WholesaleItemAvailabilityRepositoryFactory>();
        services.AddTransient<IItemAssortmentSalesChannelRepository, ItemAssortmentSalesChannelRepository>();
        services.AddTransient<IItemMasterdataQualityCheckService, ItemMasterdataQualityCheckService>();
        services.AddTransient<IQualityCheckAiConsumerService, QualityCheckAiConsumerService>();
        services.AddTransient<IIssueEntityResolver, IssueEntityResolver>();
        services.AddTransient<IIssueSeverityResolver, IssueSeverityResolver>();
        services.AddTransient<IActualValueResolver, ActualValueResolver>();

        return services;
    }

    private static IServiceCollection AddSupplierServices(this IServiceCollection services)
    {
        services.AddTransient<ISupplierRepository, SupplierRepository>();

        return services;
    }

    private static IServiceCollection AddCustomerServices(this IServiceCollection services)
    {
        services.AddTransient<IWholesaleCustomerRepositoryFactory, WholesaleCustomerRepositoryFactory>();
        services.AddTransient<IE1CustomerRepository, E1CustomerRepository>();

        return services;
    }

    private static IServiceCollection AddCanceledOrderServices(this IServiceCollection services)
    {
        services.AddTransient<ICanceledOrderLinesResolver, OrderNotificationRecipientResolver>();
        services.AddTransient<ICanceledOrderLinesResolver, ItemDescriptionResolver>();
        services.AddTransient<ICanceledOrderLinesResolver, CustomerNotificationSettingsResolver>();

        return services;
    }

    private static IServiceCollection AddHeldOrderServices(this IServiceCollection services)
    {
        services.AddTransient<IE1HeldOrderService, E1HeldOrderService>();
        services.AddTransient<IE1HeldOrderEmailService, E1HeldOrderEmailService>();
        services.AddTransient<IE1HeldOrdersEmailStatusResolver, E1HeldOrdersEmailStatusResolver>();
        services.AddTransient<IE1HeldOrdersResolver, E1HeldOrderCustomerEmailResolver>();
        services.AddTransient<IE1HeldOrdersResolver, E1HeldOrderEmployeeEmailResolver>();
        services.AddTransient<IE1HeldOrderEmailGeneratorFactory, E1HeldOrderEmailGeneratorFactory>();
        services.AddTransient<IWholesaleEmployeeRepositoryFactory, WholesaleEmployeeRepositoryFactory>();
        services.AddTransient<IE1HeldOrderRepository, E1HeldOrderRepository>();
        services.AddTransient<LvWholesaleEmployeeRepository>();
        services.AddTransient<LvHeldOrderEmailGenerator>();

        return services;
    }
}
