using MediatR;
using Tamro.Madam.Models.User.UserSettings;

namespace Tamro.Madam.Application.Commands.User.UserSettings
{
    public class GetUserSettingsCommand : IRequest<UserSettingsModel>
    {
        public string UserName { get; set; }

        public GetUserSettingsCommand(string userName)
        {
            UserName = userName;
        }
    }
}

