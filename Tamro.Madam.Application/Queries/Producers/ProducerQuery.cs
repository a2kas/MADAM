using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Producers;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Producers;

public class ProducerQuery : ProducerFilter, IRequest<PaginatedData<ProducerModel>>
{
    public ProducerSpecification Specification => new(this);
}
