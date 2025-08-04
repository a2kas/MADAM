using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items.SafetyStocks.PharmacyChains;

public class UpsertPharmacyChainCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertPharmacyChainCommand(PharmacyChainModel model)
    {
        Model = model;
    }

    public PharmacyChainModel Model { get; set; }

    public string ErrorMessage { get; set; } = "Failed to save pharmacy chain";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueIndexViolation => Result<int>.Failure($"Pharmacy chain with country '{Model.Country}' and E1 sold to '{Model.E1SoldTo}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}
