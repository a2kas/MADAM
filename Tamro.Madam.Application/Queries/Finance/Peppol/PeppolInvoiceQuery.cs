using MediatR;
using Tamro.Madam.Models.Finance.Peppol;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Finance.Peppol;

public class PeppolInvoiceQuery : PeppolInvoiceFilter, IRequest<PaginatedData<PeppolInvoiceGridModel>>
{
    public PeppolInvoiceSpecification Specification => new(this);
}
