using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Queries.Barcodes;

public class BarcodeFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<BarcodeGridModel>>? Filters { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
}
