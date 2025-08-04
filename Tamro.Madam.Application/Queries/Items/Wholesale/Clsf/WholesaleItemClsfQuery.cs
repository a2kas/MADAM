using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Items.Wholesale.Clsf;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Items.Wholesale.Clsf;

public class WholesaleItemClsfQuery : WholesaleItemClsfFilter, IRequest<PaginatedData<WholesaleItemClsfModel>>
{
}
