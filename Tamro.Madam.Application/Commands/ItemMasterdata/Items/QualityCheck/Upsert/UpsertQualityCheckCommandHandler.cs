using MediatR;
using Microsoft.EntityFrameworkCore;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.UnitOfWork;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.QualityCheck.Upsert;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UpsertQualityCheckCommandHandler : IRequestHandler<UpsertQualityCheckCommand, Result<int>>
{
    private readonly IMadamUnitOfWork _uow;

    public UpsertQualityCheckCommandHandler(IMadamUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<int>> Handle(UpsertQualityCheckCommand request, CancellationToken cancellationToken)
    {
        var existingEntity = await _uow.GetRepository<ItemQualityCheck>()
            .AsQueryable()
            .Include(x => x.ItemQualityCheckIssues)
            .FirstOrDefaultAsync(
                x => x.ItemId == request.Model.ItemId,
                cancellationToken);

        ItemQualityCheck? entity = null;

        if (existingEntity != null)
        {
            foreach (var issue in request.Model.ItemQualityCheckIssues)
            {
                var existingIssue = existingEntity.ItemQualityCheckIssues.FirstOrDefault(x => x.IssueField == issue.IssueField &&
                (x.IssueStatus == ItemQualityIssueStatus.FalsePositive || x.IssueStatus == ItemQualityIssueStatus.New));

                if (existingIssue == null)
                {
                    existingEntity.ItemQualityCheckIssues.Add(issue);
                }
            }

            entity = existingEntity;
        }
        else
        {
            entity = request.Model;
        }

        var trackedEntity = await _uow.GetRepository<ItemQualityCheck>().UpsertAsync(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(trackedEntity.Id);
    }
}
