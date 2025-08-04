using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Application.Commands.Suppliers;

public class GetSupplierCommand : IRequest<Result<SupplierDetailsModel>>, IDefaultErrorMessage
{
    public GetSupplierCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve supplier";
}
