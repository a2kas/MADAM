using Tamro.Madam.Models.General;
using Tamro.Madam.Models.State.General.Settings;

namespace Tamro.Madam.Models.State.General;

public class UserProfileStateModel
{
    public string DisplayName { get; set; }
    public string[] Permissions { get; set; }
    public string UserName { get; set; }
    public SettingsModel? Settings { get; set; }
    public BalticCountry? Country { get; set; }
}
