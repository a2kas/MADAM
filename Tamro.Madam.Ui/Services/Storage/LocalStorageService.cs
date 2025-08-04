using System.Text.Json;
using Microsoft.JSInterop;
using Tamro.Madam.Application.Services.Authentication;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Ui.Services.Storage;

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IPermissionService _permissionService;

    private const string _countryKey = "Country";

    public LocalStorageService(IJSRuntime jsRuntime, IPermissionService permissionUtils)
    {
        _jsRuntime = jsRuntime;
        _permissionService = permissionUtils;
    }

    public async Task SetCountry(string country)
    {
        await SetItem(_countryKey, country ?? "");
    }

    public async Task<BalticCountry?> ResolveCountry(string[] permissions)
    {
        BalticCountry? result = null;

        var countryString = await GetItem(_countryKey);
        var availableCountries = _permissionService.GetAvailableCountries(permissions);

        if (string.IsNullOrEmpty(countryString))
        {

            if (availableCountries.Count > 0)
            {
                result = availableCountries[0];
                await SetItem(_countryKey, result.ToString());
            }
        }
        else
        {
            if (Enum.TryParse(countryString, out BalticCountry parsedCountry))
            {
                result = parsedCountry;
            }
        }

        if (result.HasValue && !availableCountries.Contains(result.Value))
        {
            result = null;
            if (availableCountries.Count > 0)
            {
                result = availableCountries[0];
            }
            await SetItem(_countryKey, result.ToString());
        }

        return result;
    }

    private async Task<string> GetItem(string key)
    {
        string jsonValue = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

        if (!string.IsNullOrEmpty(jsonValue))
        {
            jsonValue = jsonValue.Trim('"');
            return jsonValue;
        }

        return string.Empty;
    }

    private async Task<T> GetItem<T>(string key)
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

        if (json == null)
            return default;

        return JsonSerializer.Deserialize<T>(json);
    }

    private async Task SetItem<T>(string key, T value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));
    }

    private async Task RemoveItem(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}
