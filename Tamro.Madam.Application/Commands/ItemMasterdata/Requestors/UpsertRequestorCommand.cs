using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Requestors;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Requestors;

public class UpsertRequestorCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertRequestorCommand(RequestorModel model)
    {
        Model = model;
    }

    public RequestorModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save requestor";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<int>.Failure($"Requestor with name '{Model.Name}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}
