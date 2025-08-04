using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Services.Sales.HeldOrders.Factories;
public interface IE1HeldOrderEmailGeneratorFactory
{
    IE1HeldOrderEmailGenerator Get(BalticCountry country);
}
