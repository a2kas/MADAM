using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.Brands;

public class UpsertBrandCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertBrandCommand(BrandModel model)
    {
        Model = model;
    }

    public BrandModel Model { get; set; }

    public string ErrorMessage { get; set; } = "Failed to save brand";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<int>.Failure($"Brand with name '{Model.Name}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}
