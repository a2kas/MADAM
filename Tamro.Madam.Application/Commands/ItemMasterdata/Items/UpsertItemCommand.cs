using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Models.State.General;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items;

public class UpsertItemCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertItemCommand(ItemModel model, UserProfileStateModel user)
    {
        Model = model;
        Model.EditedAt = DateTime.Now;
        Model.EditedBy = user.DisplayName;
    }

    public ItemModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save item";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<int>.Failure($"Item with name '{Model.ItemName}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}
