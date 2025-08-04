using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Commands.User.UserSettings;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Models.User.UserSettings;
using Tamro.Madam.Repository.Entities.Users.UserSettings;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.User.UserSettings;

[NoPermissionNeeded]
public class GetUserSettingsCommandHandler : IRequestHandler<GetUserSettingsCommand, UserSettingsModel>
{
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GetUserSettingsCommandHandler(IMadamUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<UserSettingsModel> Handle(GetUserSettingsCommand request, CancellationToken cancellationToken)
    {
        var userSettings = await _uow.GetRepository<UserSetting>().AsReadOnlyQueryable().SingleOrDefaultAsync(x => x.Id == request.UserName);

        return _mapper.Map<UserSettingsModel>(userSettings);
    }
}
