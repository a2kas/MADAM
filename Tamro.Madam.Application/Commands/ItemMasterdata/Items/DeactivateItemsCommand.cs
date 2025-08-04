using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.State.General;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items;
public class DeactivateItemsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeactivateItemsCommand(int[] id, UserProfileStateModel user)
    {
        Id = id;
        DisplayName = user.DisplayName;
    }

    public int[] Id { get; }
    public string DisplayName { get; }
    public string ErrorMessage { get; set; } = "Failed to deactivate items";
}
