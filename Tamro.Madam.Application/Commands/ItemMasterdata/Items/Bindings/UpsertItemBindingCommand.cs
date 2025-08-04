using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings;
using Tamro.Madam.Models.State.General;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings;

public class UpsertItemBindingCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public UpsertItemBindingCommand(ItemBindingModel model, UserProfileStateModel user)
    {
        Model = model;
        Model.EditedBy = user.DisplayName;
    }

    public ItemBindingModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save binding";
}
