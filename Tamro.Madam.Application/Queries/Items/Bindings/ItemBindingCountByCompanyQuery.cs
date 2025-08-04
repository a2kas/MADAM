using MediatR;
using Tamro.Madam.Models.Overview;

namespace Tamro.Madam.Application.Queries.Items.Bindings;

public class ItemBindingCountByCompanyQuery : IRequest<IEnumerable<CompanyBindingCountModel>>
{
}
