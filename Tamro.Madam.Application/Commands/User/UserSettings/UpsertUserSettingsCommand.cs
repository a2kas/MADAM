using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.User.UserSettings;

namespace Tamro.Madam.Application.Commands.User.UserSettings;

public class UpsertUserSettingsCommand : IRequest<Result<UserSettingsModel>>, IDefaultErrorMessage
{
    public UpsertUserSettingsCommand(UserSettingsModel user)
    {
        Model = user;
    }

    public UserSettingsModel Model { get; set; }

    public string ErrorMessage { get; set; } = "Failed to save user settings";
}
