using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.Data;
using Tamro.Madam.Models.General;
namespace Tamro.Madam.Models.Tests.Data;

[TestFixture]
public class PharmacyChainDataTests
{
    [Test]
    public void AllBalticCountries_ShouldBeFoundIn_CountryDisplayNamesDictionary()
    {
        // Arrange
        var expectedCountries = Enum.GetValues(typeof(BalticCountry))
                            .Cast<BalticCountry>()
                            .ToList();

        // Act
        var countryDisplayNames = GenericData.CountryDisplayNames;

        // Assert
        countryDisplayNames.Count.ShouldBe(expectedCountries.Count);
        foreach (var country in expectedCountries)
        {
            countryDisplayNames.ContainsKey(country.ToString()).ShouldBeTrue();
        }
    }

    [Test]
    public void AllGroups_ShouldBeFoundIn_GroupDisplayNamesDictionary()
    {
        // Arrange
        var expectedGroups = Enum.GetValues(typeof(PharmacyGroup))
                               .Cast<PharmacyGroup>()
                               .ToList();

        // Act
        var groupDisplayNames = PharmacyChainData.GroupDisplayNames;

        // Assert
        groupDisplayNames.Count.ShouldBe(expectedGroups.Count);
        foreach (var group in expectedGroups)
        {
            groupDisplayNames.ContainsKey(group.ToString()).ShouldBeTrue();
        }
    }
}
