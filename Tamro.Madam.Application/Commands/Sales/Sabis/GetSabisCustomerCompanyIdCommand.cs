using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Sales.Sabis;

public class GetSabisCustomerCompanyIdCommand : IRequest<Result<string>>, IDefaultErrorMessage
{
    public GetSabisCustomerCompanyIdCommand(int addressNumber)
    {
        AddressNumber = addressNumber;
    }

    public int AddressNumber { get; }
    public string ErrorMessage { get; set; } = "Failed to retrieve company id";
}