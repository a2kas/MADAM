using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Forms;

namespace Tamro.Madam.Application.Queries.Forms;

public class FormFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<FormModel>>? Filters { get; set; }
    public string SearchTerm { get; set; }
}
