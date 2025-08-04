using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Common.Constants.SearchExpressionConstants;
using Tamro.Madam.Models.ItemMasterdata.MeasurementUnits;
using Tamro.Madam.Repository.Entities.ItemMasterdata.MeasurementUnits;

namespace Tamro.Madam.Application.Queries.MeasurementUnits;

public class MeasurementUnitSpecification : Specification<MeasurementUnit>
{
    public MeasurementUnitSpecification(MeasurementUnitFilter filter)
    {
        if (filter.Filters == null)
        {
            return;
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null || (appliedFilter.Operator != SearchStringConstants.IsNotEmpty && appliedFilter.Operator != SearchStringConstants.IsEmpty && appliedFilter.Value == null))
            {
                continue;
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(MeasurementUnitModel.Name)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Name);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.Name.Contains(filter.SearchTerm));
        }
    }
}
