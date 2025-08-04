using MudBlazor;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;

namespace Tamro.Madam.Application.Queries.MeasurementUnits;

public class MeasurementUnitFilter : PaginationFilter
{
    public ICollection<IFilterDefinition<MeasurementUnitModel>>? Filters { get; set; }
    public string SearchTerm { get; set; }
}
