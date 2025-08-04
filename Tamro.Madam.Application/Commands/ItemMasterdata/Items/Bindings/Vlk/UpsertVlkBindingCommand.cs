using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Vlk;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.Bindings.Vlk;

public class UpsertVlkBindingCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertVlkBindingCommand(VlkBindingDetailsModel model)
    {
        Model = model;
    }

    public VlkBindingDetailsModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save Vlk binding";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueIndexViolation => Result<int>.Failure($"Binding for the provided NpakId7 already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}
