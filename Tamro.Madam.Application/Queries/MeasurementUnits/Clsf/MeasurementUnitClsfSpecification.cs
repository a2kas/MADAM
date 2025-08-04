using Ardalis.Specification;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;

namespace Tamro.Madam.Application.Queries.MeasurementUnits.Clsf;

public class MeasurementUnitClsfSpecification : Specification<MeasurementUnit>
{
    public MeasurementUnitClsfSpecification(MeasurementUnitClsfFilter filter)
    {
        Query.Where(x => x.Name!.Contains(filter.SearchTerm),
            !string.IsNullOrEmpty(filter.SearchTerm));
    }
}
