using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.MeasurementUnits;

public class UpsertMeasurementUnitCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertMeasurementUnitCommand(MeasurementUnitModel model)
    {
        Model = model;
    }

    public MeasurementUnitModel Model { get; set; }

    public string ErrorMessage { get; set; } = "Failed to save measurement unit";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<int>.Failure($"Measurement unit with name '{Model.Name}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}