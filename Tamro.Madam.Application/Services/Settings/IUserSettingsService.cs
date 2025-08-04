using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.User.UserSettings;

namespace Tamro.Madam.Application.Services.Settings;

public interface IUserSettingsService
{
    Task<Result<UserSettingsModel>> ResolveUserSettings(string userId);
}
