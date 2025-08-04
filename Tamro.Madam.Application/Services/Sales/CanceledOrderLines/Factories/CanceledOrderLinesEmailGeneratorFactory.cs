using Microsoft.Extensions.DependencyInjection;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Factories;

public class CanceledOrderLinesEmailGeneratorFactory : ICanceledOrderLinesEmailGeneratorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CanceledOrderLinesEmailGeneratorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ICanceledOrderLinesEmailGenerator Get(BalticCountry country)
    {
        return country switch
        {
            BalticCountry.LV => _serviceProvider.GetService<LvCanceledOrderLinesEmailGenerator>(),
            BalticCountry.LT => _serviceProvider.GetService<LtCanceledOrderLinesEmailGenerator>(),
            BalticCountry.EE => _serviceProvider.GetService<EeCanceledOrderLinesEmailGenerator>(),
            _ => throw new NotSupportedException($"Country '{country}' not supposed")
        };
    }
}
