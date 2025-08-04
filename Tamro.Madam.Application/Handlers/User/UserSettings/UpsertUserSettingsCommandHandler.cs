using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Commands.User.UserSettings;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Infrastructure.Session;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.User.UserSettings;
using Tamro.Madam.Repository.Entities.Users.UserSettings;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Handlers.User.UserSettings;

[NoPermissionNeeded]
public class UpsertUserSettingsCommandHandler : IRequestHandler<UpsertUserSettingsCommand, Result<UserSettingsModel>>
{
    private readonly IUserContext _userContext;
    private readonly IMadamUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpsertUserSettingsCommandHandler(IUserContext userContext, IMadamUnitOfWork uow, IMapper mapper)
    {
        _userContext = userContext;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<UserSettingsModel>> Handle(UpsertUserSettingsCommand request, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<UserSetting>();
        var entity = _mapper.Map<UserSetting>(request.Model);
        var existingUserSettings = await repo.AsQueryable().SingleOrDefaultAsync(x => x.Id == _userContext.UserName);
        if (existingUserSettings == null)
        {
            var newUserSettings = _mapper.Map<UserSetting>(request.Model);
            newUserSettings.Id = _userContext.UserName;

            repo.Create(newUserSettings);
        }
        else
        {
            _mapper.Map(request.Model, existingUserSettings);
        }

        await _uow.SaveChangesAsync(cancellationToken);

        return Result<UserSettingsModel>.Success(request.Model);
    }
}
