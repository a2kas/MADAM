using System;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Ui.ComponentExtensions
{
    public static class BlazorFlagExtensions
    {
        public static Blazor.Flags.Country GetBlazorFlagCountry(LanguageCountry? country)
        {
            if (country == null)
            {
                return Blazor.Flags.Country.EU;
            }

            return country switch
            {
                LanguageCountry.EE => Blazor.Flags.Country.EE,
                LanguageCountry.LV => Blazor.Flags.Country.LV,
                LanguageCountry.LT => Blazor.Flags.Country.LT,
                LanguageCountry.EN => Blazor.Flags.Country.GB,
                _ => Blazor.Flags.Country.UN,
            };
        }

        public static Blazor.Flags.Country GetBlazorFlagCountry<T>(T? country) where T : struct, Enum
        {
            if (typeof(T) == typeof(LanguageCountry))
            {
                return GetBlazorFlagCountry(country as LanguageCountry?);
            }
            else
            {
                LanguageCountry languageCountry = ConvertToLanguageCountry(country);

                return GetBlazorFlagCountry(languageCountry);
            }
        }

        public static LanguageCountry ConvertToLanguageCountry<T>(T? country) where T : struct, Enum
        {
            Type enumType = typeof(T);

            string[] enumNames = Enum.GetNames(enumType);

            foreach (string name in enumNames)
            {
                if (Enum.TryParse(name, out LanguageCountry result) && result.ToString() == country.ToString())
                {
                    return result;
                }
            }

            throw new ArgumentException($"Unsupported enum type: {enumType}");
        }
    }
}
