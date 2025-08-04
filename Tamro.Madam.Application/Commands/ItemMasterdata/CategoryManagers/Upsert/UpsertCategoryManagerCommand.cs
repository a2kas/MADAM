using Microsoft.Data.SqlClient;
using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;
using Tamro.Madam.Repository.Entities.ItemMasterdata.CategoryManagers;
using Tamro.Madam.Repository.UnitOfWork.ExceptionHandling.SqlErrorConvertors;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Upsert;
public class UpsertCategoryManagerCommand : IRequest<Result<int>>, ICustomExceptionHandler<Result<int>>
{
    public int? Id { get; set; }
    public required string EmailAddress { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required BalticCountry Country { get; set; }

    public DateTime? RowVer { get; set; }

    private string _defaultErrorMessage { get; set; } = "Failed to insert or update a category manager";

    public Result<int> HandleException(Exception exception)
    {
        string message = _defaultErrorMessage;
        if (exception.InnerException is SqlException sqlEx)
        {
            message = sqlEx.ConvertToUserFriendlyMessage<CategoryManager>(_defaultErrorMessage);
        }
        return new Result<int>
        {
            Succeeded = false,
            Errors = [message],
        };
    }
}
