using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.Jpg;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Commands.Hr.Jira.Administration;

[RequiresPermission(Permissions.CanManageJira)]
public class SetAccountsIsActivatedCommandHandler : IRequestHandler<SetAccountsIsActivatedCommand, Result<int>>
{
    private readonly IJpgUnitOfWork _uow;

    public SetAccountsIsActivatedCommandHandler(IJpgUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(SetAccountsIsActivatedCommand request, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<JiraAccount>();
        var entities = await repo.AsQueryable()
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        entities.ForEach(entity => entity.IsActive = request.IsActivated);

        var result = await _uow.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(result);
    }
}
