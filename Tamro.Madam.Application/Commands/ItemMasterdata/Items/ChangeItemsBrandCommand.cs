using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Brands;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Items;

public class ChangeItemsBrandCommand : IRequest<Result<IEnumerable<ItemModel>>>, IDefaultErrorMessage, ICustomExceptionHandler<Result<IEnumerable<ItemModel>>>
{
    public ChangeItemsBrandCommand(IEnumerable<ItemModel> items, BrandClsfModel newBrand)
    {
        Items = items;
        NewBrand = newBrand;
    }

    public IEnumerable<ItemModel> Items { get; set; }
    public BrandClsfModel NewBrand { get; set; }
    public string ErrorMessage { get; set; } = "Failed to change brand";

    public Result<IEnumerable<ItemModel>> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueConstraintViolation => Result<IEnumerable<ItemModel>>.Failure($"Failed to change brand for one of items, because brand already has such item"),
                _ => throw exception,
            };
        }

        return null;
    }
}