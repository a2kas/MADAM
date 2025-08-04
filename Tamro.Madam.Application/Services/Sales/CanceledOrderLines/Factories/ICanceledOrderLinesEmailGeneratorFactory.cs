using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Services.Sales.CanceledOrderLines.Factories;

public interface ICanceledOrderLinesEmailGeneratorFactory
{
    ICanceledOrderLinesEmailGenerator Get(BalticCountry country);
}
