using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Items.SafetyStocks.PharmacyChains;

public class PharmacyChainQuery : PharmacyChainFilter, IRequest<PaginatedData<PharmacyChainModel>>
{
    public PharmacyChainSpecification Specification => new(this);
}
