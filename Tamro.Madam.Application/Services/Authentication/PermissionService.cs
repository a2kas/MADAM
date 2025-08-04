using Tamro.Madam.Application.Access;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Services.Authentication;

public class PermissionService : IPermissionService
{
    public List<BalticCountry> GetAvailableCountries(string[] permissions)
    {
        var result = new List<BalticCountry>();

        if (permissions.Contains(Permissions.CountryLv))
        {
            result.Add(BalticCountry.LV);
        }

        if (permissions.Contains(Permissions.CountryEe))
        {
            result.Add(BalticCountry.EE);
        }

        if (permissions.Contains(Permissions.CountryLt))
        {
            result.Add(BalticCountry.LT);
        }

        return result;
    }
}
