using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Producers;

public class UpsertProducerCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertProducerCommand(ProducerModel model)
    {
        Model = model;
    }

    public ProducerModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save producer";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<int>.Failure($"Producer with name '{Model.Name}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}
