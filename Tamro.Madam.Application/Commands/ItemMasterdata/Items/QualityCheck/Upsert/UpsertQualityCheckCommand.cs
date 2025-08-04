using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.QualityCheck.Upsert;

public class UpsertQualityCheckCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public ItemQualityCheck Model { get; set; }

    public string ErrorMessage { get; set; } = "Failed to insert or update item quality check";
}
