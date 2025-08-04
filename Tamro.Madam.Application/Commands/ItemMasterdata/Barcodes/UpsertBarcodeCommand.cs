using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Barcodes;

public class UpsertBarcodeCommand : IRequest<Result<int>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<int>>
{
    public UpsertBarcodeCommand(BarcodeModel model)
    {
        Model = model;
        Model.RowVer = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc); //TODO: Change this once DB is adjusted
    }

    public BarcodeModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save barcode";

    public Result<int> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<int>.Failure($"Barcode with EAN code '{Model.Ean}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}
