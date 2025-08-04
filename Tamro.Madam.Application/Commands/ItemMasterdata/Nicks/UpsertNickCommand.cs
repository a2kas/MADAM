using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Nicks;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Nicks;

public class UpsertNickCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertNickCommand(NickModel model)
    {
        Model = model;
    }

    public NickModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save baltic nick";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<int>.Failure($"Baltic nick with name '{Model.Name}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}
