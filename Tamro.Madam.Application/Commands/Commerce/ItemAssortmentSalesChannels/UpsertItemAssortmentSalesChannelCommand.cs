using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Commerce.ItemAssortmentSalesChannels;
using Tamro.Madam.Repository.Constants;

namespace Tamro.Madam.Application.Commands.Commerce.ItemAssortmentSalesChannels;

public class UpsertItemAssortmentSalesChannelCommand : IRequest<Result<ItemAssortmentSalesChannelDetailsModel>>,
    IDefaultErrorMessage,
    ICustomExceptionHandler<Result<ItemAssortmentSalesChannelDetailsModel>>
{
    public UpsertItemAssortmentSalesChannelCommand(ItemAssortmentSalesChannelDetailsModel model)
    {
        Model = model;
    }

    public ItemAssortmentSalesChannelDetailsModel Model { get; set; }

    public string ErrorMessage { get; set; } = "Failed to save item assortment sales channel";

    public Result<ItemAssortmentSalesChannelDetailsModel> HandleException(Exception exception)
    {
        if (exception.InnerException is SqlException sqlEx)
        {
            return sqlEx.Number switch
            {
                (int)MsSqlErrorNumber.UniqueIndexViolation => Result<ItemAssortmentSalesChannelDetailsModel>
                    .Failure($"Item assortment sales channel with name '{Model.Name}' and country '{Model.Country}' already exists"),
                _ => throw exception,
            };
        }

        return null;
    }
}