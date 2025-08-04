using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Hr.Jira.Administration;
using Tamro.Madam.Repository.Entities.Jpg;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Commands.Hr.Jira.Administration;

[RequiresPermission(Permissions.CanManageJira)]
public class UpdateAccountsCommandHandler : IRequestHandler<UpdateAccountsCommand, Result<JiraAccountModel>>
{
    private readonly IJpgUnitOfWork _uow;
    private readonly IMapper _mapper;

    public UpdateAccountsCommandHandler(IJpgUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<Result<JiraAccountModel>> Handle(UpdateAccountsCommand request, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<JiraAccount>();
        var entity = await repo.AsQueryable()
            .Where(x => x.Id == request.Model.Id)
            .FirstAsync(cancellationToken);

        _mapper.Map(request.Model, entity);

        var updatedEntity = await repo.UpsertAsync(entity);

        var updateResult = await _uow.SaveChangesAsync(cancellationToken);

        if (updateResult > 0)
        {
            var model = _mapper.Map<JiraAccountModel>(updatedEntity);
            return Result<JiraAccountModel>.Success(model);
        }

        return Result<JiraAccountModel>.Failure("Failed to update Jira account");
    }
}
