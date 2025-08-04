using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.Requestors;

public class DeleteRequestorsCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public DeleteRequestorsCommand(int[] id)
    {
        Id = id;
    }

    public int[] Id { get; }
    public string ErrorMessage { get; set; } = "Failed to delete requestors";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.ForeignKeyOrReferenceContstraintViolation => Result<int>.Failure($"Could not delete because the requestor has items assigned"),
                _ => throw exception,
            };
        }

        return null;
    }
}
