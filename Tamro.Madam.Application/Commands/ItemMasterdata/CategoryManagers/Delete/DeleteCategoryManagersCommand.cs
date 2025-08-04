using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Delete;
public class DeleteCategoryManagersCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteCategoryManagersCommand(IEnumerable<int> ids)
    {
        Ids = [.. ids];
    }

    public List<int> Ids { get; set; }

    public string ErrorMessage { get; set; } = "Failed to delete a category manager";
}
