using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Items.SafetyStocks;

public class SafetyStockQuery : SafetyStockFilter, IRequest<PaginatedData<SafetyStockGridDataModel>>
{
    public SafetyStockSpecification Specification => new(this);
}
