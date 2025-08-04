using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Sales.Sabis;
using Tamro.Madam.Models.State.General;

namespace Tamro.Madam.Application.Commands.Sales.Sabis;

public class UpsertSksContractCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public UpsertSksContractCommand(SksContractModel model, UserProfileStateModel user)
    {
        Model = model;
        model.EditedBy = user.DisplayName;
        model.EditedAt = DateTime.Now;
    }

    public SksContractModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save contract mapping";
}
