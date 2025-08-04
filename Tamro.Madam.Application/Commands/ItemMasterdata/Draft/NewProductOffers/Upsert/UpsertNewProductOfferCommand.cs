using MediatR;
using Microsoft.Data.SqlClient;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.Repository.UnitOfWork.ExceptionHandling.SqlErrorConvertors;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.Draft.NewProductOffers.Upsert;
public class UpsertNewProductOfferCommand
    : IRequest<Result<UpsertNewProductOfferResult>>,
      ICustomExceptionHandler<Result<UpsertNewProductOfferResult>>
{
    public int? Id { get; set; }
    public DateTime? RowVer { get; set; }
    public required int SupplierId { get; set; }
    public int? ItemCategoryManagerId { get; set; }
    public NewProductOfferStatus Status { get; set; } = NewProductOfferStatus.New;
    public required BalticCountry Country { get; set; }
    public required FileWithName File { get; set; }


    private string _defaultErrorMessage { get; set; } = "Failed to insert or update new product offer";

    public Result<UpsertNewProductOfferResult> HandleException(Exception exception)
    {
        string message = _defaultErrorMessage;
        if (exception.InnerException is SqlException sqlEx)
        {
            message = sqlEx.ConvertToUserFriendlyMessage<NewProductOffer>(_defaultErrorMessage);
        }
        return new Result<UpsertNewProductOfferResult>
        {
            Succeeded = false,
            Errors = [message],
        };
    }
}
