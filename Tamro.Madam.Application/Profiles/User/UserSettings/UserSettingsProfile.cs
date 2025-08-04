using AutoMapper;
using Tamro.Madam.Models.State.General.Settings;
using Tamro.Madam.Models.User.UserSettings;
using Tamro.Madam.Repository.Entities.Users.UserSettings;

namespace Tamro.Madam.Application.Profiles.User.UserSettings;

public class UserSettingsProfile : Profile
{
    public UserSettingsProfile()
    {
        CreateMap<UserSetting, UserSettingsModel>()
            .ForMember(d => d.Usability, o => o.MapFrom(src => new UsabilitySettingsModel
            {
                RowsPerPage = src.RowsPerPage
            }));
        CreateMap<UserSettingsModel, UserSetting>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.RowsPerPage, o => o.MapFrom(src => src.Usability.RowsPerPage));
        CreateMap<UsabilitySettingsModel, UserUsabilitySettingsModel>().ReverseMap();
        CreateMap<UserSettingsModel, SettingsModel>();
        CreateMap<UserSettingsModel, UserSettingsModel>();
    }
}
