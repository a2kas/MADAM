namespace Tamro.Madam.Models.General;

public static class Classifiers
{
    public static string EeWholesaleCode = "00501";
    public static string EeRetailCode = "00505";
    public static string EeMagnumCode = "11332521";
    public static string LvWholesaleCode = "00601";
    public static string LvRetailCode = "00605";
    public static string LtWholesaleCode = "00701";
    public static string LtRetailCode = "00705";
    public static string LtOrtopedijosTechnikaCode = "132150379";
    public static string LtUabAccDistributionCode = "135150085";

    public static IEnumerable<Company> Companies = new List<Company>
    {
        new()
        {
            Id = 1,
            Value = EeWholesaleCode,
            Name = "EE Wholesale",
            Country = BalticCountry.EE,
        },
        new()
        {
            Id = 2,
            Value = EeRetailCode,
            Name = "EE Retail",
            Country = BalticCountry.EE,
        },
        new()
        {
            Id = 10,
            Value = EeMagnumCode,
            Name = "Magnum EE",
            Country = BalticCountry.EE,
        },
        new()
        {
            Id = 3,
            Value = LvWholesaleCode,
            Name = "LV Wholesale",
            Country = BalticCountry.LV,
        },
        new()
        {
            Id = 4,
            Value = LvRetailCode,
            Name = "LV Retail",
            Country = BalticCountry.LV,
        },
        new()
        {
            Id = 5,
            Value = LtWholesaleCode,
            Name = "LT Wholesale",
            Country = BalticCountry.LT,
        },
        new()
        {
            Id = 6,
            Value = LtRetailCode,
            Name = "LT Retail",
            Country = BalticCountry.LT,
        },
        new()
        {
            Id = 9,
            Value = LtOrtopedijosTechnikaCode,
            Name = "Ortopedijos technika",
            Country = BalticCountry.LT,
        },
        new()
        {
            Id = 7,
            Value = LtUabAccDistributionCode,
            Name = "UAB ACC Distribution",
            Country = BalticCountry.LT,
        }
    };

    public static IEnumerable<CountryModel> Countries = new List<CountryModel>()
    {
        new()
        {
            Name = "EE",
            Value = BalticCountry.EE,
        },
        new()
        {
            Name = "LV",
            Value = BalticCountry.LV,
        },
        new()
        {
            Name = "LT",
            Value = BalticCountry.LT,
        },
    };

    public static List<Language> Languages = new()
    {
        new()
        {
            Id = 1,
            Code = LanguageCountry.LT,
            Name = "Lithuanian",
        },
        new()
        {
            Id = 2,
            Code = LanguageCountry.LV,
            Name = "Latvian",
        },
        new()
        {
            Id = 3,
            Code = LanguageCountry.EE,
            Name = "Estonian",
        },
        new()
        {
            Id = 4,
            Code = LanguageCountry.EN,
            Name = "English",
        },
        new()
        {
            Id = 5,
            Code = LanguageCountry.NA,
            Name = "N/A",
        }
    };

    public static string GetCountryDisplayName(BalticCountry country)
    {
        switch (country)
        {
            case BalticCountry.LT:
                return "Lithuania";
            case BalticCountry.LV:
                return "Latvia";
            case BalticCountry.EE:
                return "Estonia";
            default:
                return string.Empty;
        }
    }
}
