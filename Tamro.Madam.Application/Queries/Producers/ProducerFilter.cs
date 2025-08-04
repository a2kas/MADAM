using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Producers;

namespace Tamro.Madam.Application.Queries.Producers;

public class ProducerFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<ProducerModel>>? Filters { get; set; }
    public string SearchTerm { get; set; }
}
