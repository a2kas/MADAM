using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Services.Authentication;

public interface IPermissionService
{
    List<BalticCountry> GetAvailableCountries(string[] permissions);
}