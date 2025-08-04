using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.ItemMasterdata.Barcodes;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Barcodes;

namespace Tamro.Madam.Application.Queries.Barcodes;

public class BarcodeSpecification : Specification<Barcode>
{
    public BarcodeSpecification(BarcodeFilter filter)
    {
        if (filter.Filters == null)
        {
            return;
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(BarcodeGridModel.Ean)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Ean);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(BarcodeGridModel.ItemName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Item.ItemName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(BarcodeGridModel.ItemId)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.Item.Id);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(BarcodeGridModel.Measure)))
            {
                Query.ApplyBoolFilter((bool?)appliedFilter.Value, x => x.Measure);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(BarcodeGridModel.RowVer)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.RowVer);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.Ean.Contains(filter.SearchTerm) || x.Item.ItemName.Contains(filter.SearchTerm));
        }
    }
}
