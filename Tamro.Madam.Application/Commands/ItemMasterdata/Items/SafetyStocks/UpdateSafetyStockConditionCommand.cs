using FluentValidation;
using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks;

public class UpdateSafetyStockConditionCommand : IRequest<Result<SafetyStockConditionModel>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<SafetyStockConditionModel>>
{
    public UpdateSafetyStockConditionCommand(SafetyStockConditionUpsertModel request)
    {
        Model = request;
    }

    public SafetyStockConditionUpsertModel Model { get; set; }

    public string ErrorMessage { get; set; } = "Failed to save safety stock";

    public Result<SafetyStockConditionModel> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<SafetyStockConditionModel>.Failure($"Safety stock with id '{Model.Id}' already exists"),
                (int)MsSqlErrorNumber.UniqueIndexViolation => Result<SafetyStockConditionModel>.Failure($"Such safety stock condition already exists"),
                _ => Result<SafetyStockConditionModel>.Failure(ErrorMessage),
            };
        }
        if (exception is ValidationException validationException)
        {
            return Result<SafetyStockConditionModel>.Failure(validationException.Message);
        }

        return null;
    }
}
