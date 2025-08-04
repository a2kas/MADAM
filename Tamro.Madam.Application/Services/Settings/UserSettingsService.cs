using MediatR;
using Tamro.Madam.Application.Commands.User.UserSettings;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Common.Constants;
using Tamro.Madam.Models.User.UserSettings;

namespace Tamro.Madam.Application.Services.Settings;

public class UserSettingsService : IUserSettingsService
{
    private readonly IMediator _mediator;

    public UserSettingsService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<Result<UserSettingsModel>> ResolveUserSettings(string userName)
    {
        var userSettingsModel = await _mediator.Send(new GetUserSettingsCommand(userName));

        if (!HasValidUserSettings(userSettingsModel))
        {
            var defaultUserSettingsModel = new UserSettingsModel
            {
                Usability = new UserUsabilitySettingsModel
                {
                    RowsPerPage = UserSettingsConstants.DefaultRowsPerPage,
                }
            };

            return await _mediator.Send(new UpsertUserSettingsCommand(defaultUserSettingsModel));
        }

        return Result<UserSettingsModel>.Success(userSettingsModel);
    }

    private static bool HasValidUserSettings(UserSettingsModel userSettingsModel)
    {
        return userSettingsModel is not null &&
            userSettingsModel.Usability.RowsPerPage is not null;
    }
}
