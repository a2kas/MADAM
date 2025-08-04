using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Forms;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Forms;

public class UpsertFormCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertFormCommand(FormModel model)
    {
        Model = model;
    }

    public FormModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save form";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<int>.Failure($"Form with name '{Model.Name}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}
