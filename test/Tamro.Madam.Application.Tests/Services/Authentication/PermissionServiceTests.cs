using NUnit.Framework;
using Shouldly;
using Tamro.Madam.Application.Services.Authentication;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Tests.Services.Authentication;

[TestFixture]
public class PermissionServiceTests
{
    private PermissionService _permissionService;

    [SetUp]
    public void SetUp()
    {
        _permissionService = new PermissionService();
    }

    [TestCase(new string[] { "Country.LV", "Country.LT", "Country.EE" }, new[] { BalticCountry.LV, BalticCountry.EE, BalticCountry.LT })]
    [TestCase(new string[] { "Country.LV", "Country.XX" }, new[] { BalticCountry.LV })]
    [TestCase(new string[] { "Something" }, new BalticCountry[] { })]
    [TestCase(new string[] { }, new BalticCountry[] { })]
    public void GetAvailableCountries_CollectsCountriesFromArray(string[] stringArray, BalticCountry[] expectedCountries)
    {
        // Act
        var result = _permissionService.GetAvailableCountries(stringArray);

        // Assert
        result.ShouldBe(expectedCountries);
    }
}
