using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.MeasurementUnits;

public class DeleteMeasurementUnitsCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteMeasurementUnitsCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete measurement units";
}