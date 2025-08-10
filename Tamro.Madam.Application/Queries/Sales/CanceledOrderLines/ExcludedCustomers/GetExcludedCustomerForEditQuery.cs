using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Sales.CanceledOrderLines.ExcludedCustomers;

namespace Tamro.Madam.Application.Queries.Sales.CanceledOrderLines.ExcludedCustomers;

public class GetExcludedCustomerForEditQuery : IRequest<Result<ExcludedCustomerDetailsModel>>, IDefaultErrorMessage
{
    public GetExcludedCustomerForEditQuery(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve excluded customer details";
}