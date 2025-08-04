using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Application.Commands.Suppliers;

public class UpsertSupplierCommand : IRequest<Result<SupplierDetailsModel>>, IDefaultErrorMessage
{
    public UpsertSupplierCommand(SupplierDetailsModel model)
    {
        Model = model;
    }

    public SupplierDetailsModel Model { get; set; }

    public string ErrorMessage { get; set; } = "Failed to save supplier";
}
