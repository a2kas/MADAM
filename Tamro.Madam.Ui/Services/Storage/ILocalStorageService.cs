using Tamro.Madam.Models.General;

namespace Tamro.Madam.Ui.Services.Storage;

public interface ILocalStorageService
{
    Task SetCountry(string country);
    Task<BalticCountry?> ResolveCountry(string[] permissions);
}
