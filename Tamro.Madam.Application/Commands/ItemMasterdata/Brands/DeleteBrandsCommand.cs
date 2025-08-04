using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Brands;

public class DeleteBrandsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteBrandsCommand(List<int> id)
    {
        Id = id;
    }

    public List<int> Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete brands";
}
