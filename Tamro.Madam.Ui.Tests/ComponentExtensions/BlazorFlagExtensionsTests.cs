using Shouldly;
using NUnit.Framework;
using Tamro.Madam.Models.General;
using Tamro.Madam.Ui.ComponentExtensions;

namespace Tamro.Madam.Ui.Tests.ComponentExtensions;

[TestFixture]
public class BlazorFlagExtensionsTests
{
    [TestCase(LanguageCountry.EE, Blazor.Flags.Country.EE)]
    [TestCase(LanguageCountry.LV, Blazor.Flags.Country.LV)]
    [TestCase(LanguageCountry.LT, Blazor.Flags.Country.LT)]
    [TestCase(LanguageCountry.EN, Blazor.Flags.Country.GB)]
    [TestCase(LanguageCountry.NA, Blazor.Flags.Country.UN)]
    public void GetBlazorFlagCountry_Gets_Correct_BlazorFlagCountry_WhenValueIsOfLanguageCountryType(LanguageCountry? src, Blazor.Flags.Country expectedBlazorFlagCountry)
    {
        // Act
        var blazorFlagCountry = BlazorFlagExtensions.GetBlazorFlagCountry(src);

        // Assert
        blazorFlagCountry.ShouldBe(expectedBlazorFlagCountry);
    }

    [TestCase(BalticCountry.EE, Blazor.Flags.Country.EE)]
    [TestCase(BalticCountry.LV, Blazor.Flags.Country.LV)]
    [TestCase(BalticCountry.LT, Blazor.Flags.Country.LT)]
    public void GetBlazorFlagCountry_Gets_Correct_BlazorFlagCountry_WhenValueIsNotOfLanguageCountryType(BalticCountry? src, Blazor.Flags.Country expectedBlazorFlagCountry)
    {
        // Act
        var blazorFlagCountry = BlazorFlagExtensions.GetBlazorFlagCountry(src);

        // Assert
        blazorFlagCountry.ShouldBe(expectedBlazorFlagCountry);
    }

    [TestCase(BalticCountry.EE, LanguageCountry.EE)]
    [TestCase(BalticCountry.LV, LanguageCountry.LV)]
    [TestCase(BalticCountry.LT, LanguageCountry.LT)]
    public void ConvertToLanguageCountry_Converts_Enum_Value_To_LanguageCountry(BalticCountry? src, LanguageCountry expectedCountry)
    {
        // Act
        var result = BlazorFlagExtensions.ConvertToLanguageCountry<BalticCountry>(src);

        // Assert
        result.ShouldBe(expectedCountry);
    }

    [Test]
    public void ConvertToLanguageCountry_Throws_Exception_For_Invalid_Enum_Value()
    {
        // Arrange
        TestDelegate testDelegate = () => BlazorFlagExtensions.ConvertToLanguageCountry<BalticCountry>((BalticCountry)100);

        // Act & Assert
        Assert.Throws<ArgumentException>(testDelegate);
    }
}
