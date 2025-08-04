using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Producers.Clsf;

public class ProducerClsfQuery : ProducerClsfFilter, IRequest<PaginatedData<ProducerClsfModel>>
{
    public ProducerClsfSpecification Specification => new(this);
}
