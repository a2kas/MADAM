using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.ItemMasterdata.Items;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items;

namespace Tamro.Madam.Application.Queries.Items;

public class ItemSpecification : Specification<Item>
{
    public ItemSpecification(ItemFilter filter)
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

            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.Id)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.Id);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.ItemName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ItemName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.Description)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Description);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.Producer)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Producer.Name);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.Brand)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Brand.Name);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.Strength)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Strength);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.Form)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Form.Name);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.AtcName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Atc.Name);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.AtcCode)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Atc.Value);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.SupplierNick)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.SupplierNick.Name);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.Measure)))
            {
                Query.ApplyDecimalFilter(appliedFilter.Operator, appliedFilter.Value, x => x.Measure);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.MeasurementUnit)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.MeasurementUnit.Name);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.Numero)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.Numero);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.ActiveSubstance)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ActiveSubstance);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemGridModel.Active)))
            {
                Query.ApplyBoolFilter((bool?)appliedFilter.Value, x => x.Active);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.ItemName!.Contains(filter.SearchTerm) || x.Description!.Contains(filter.SearchTerm) || (x.Id.ToString() ?? "").Contains(filter.SearchTerm));
        }
    }
}
