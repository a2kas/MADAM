using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders.Factories;
public class E1HeldOrderEmailGeneratorFactory : IE1HeldOrderEmailGeneratorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public E1HeldOrderEmailGeneratorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IE1HeldOrderEmailGenerator Get(BalticCountry country)
    {
        return country switch
        {
            BalticCountry.LV => _serviceProvider.GetService<LvHeldOrderEmailGenerator>(),
            _ => throw new NotSupportedException($"Country '{country}' not supposed")
        };
    }
}
