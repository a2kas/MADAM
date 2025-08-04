using MediatR;
using Tamro.Madam.Models.ItemMasterdata.Items.Bindings.Retail;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Items.Bindings.Retail;

public class GeneratedRetailCodeQuery : GeneratedRetailCodeFilter, IRequest<PaginatedData<GeneratedRetailCodeModel>>
{
    public GeneratedRetailCodeSpecification Specification => new(this);
}
