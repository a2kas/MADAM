using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Queries.Items.SafetyStocks;

public class SafetyStockSpecification : Specification<SafetyStockGridData>
{
    public SafetyStockSpecification(SafetyStockFilter filter)
    {
        if (filter.Filters == null)
        {
            return;
        }
        if (filter.Country != null)
        {
            Query.Where(x => x.Country == filter.Country);
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.CanBuy)))
            {
                Query.ApplyBoolFilter((bool?)appliedFilter.Value, x => x.CanBuy);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.Id)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.Id);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.CheckDays)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.CheckDays);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.WholesaleQuantity)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => (int)x.WholesaleQuantity);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.QuantityToBuy)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => (int)x.QuantityToBuy);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.RetailQuantity)))
            {
                Query.ApplyDecimalFilter(appliedFilter.Operator, appliedFilter.Value, x => x.RetailQuantity);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.PharmacyChainDisplayName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.PharmacyChainDisplayName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.SafetyStockPharmacyChainGroup)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.SafetyStockPharmacyChainGroup);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.ItemName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ItemName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.ItemNo)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ItemNo);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.ItemGroup)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ItemGroup);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.ProductClass)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ProductClass);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.Brand)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Brand);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.SupplierNumber)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => (int)x.SupplierNumber);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.Cn3)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Cn3);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.Cn1)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Cn1);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.SupplierNick)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.SupplierNick);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.Substance)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Substance);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SafetyStockGridDataModel.Comment)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Comment);
            }
        }
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            string searchTermLower = filter.SearchTerm.ToLower();

            if (Enum.GetNames(typeof(BalticCountry)).Any(c => c.ToLower().Contains(searchTermLower)))
            {
                var matchingCountries = Enum.GetValues(typeof(BalticCountry)).Cast<BalticCountry>().Where(c => c.ToString().ToLower().Contains(searchTermLower));

                Query.Where(x => matchingCountries.Contains(x.Country));
            }
            else
            {
                Query.Where(x => x.Id.ToString().Contains(filter.SearchTerm) || x.SafetyStockPharmacyChainGroup.Contains(filter.SearchTerm) || x.PharmacyChainDisplayName.Contains(filter.SearchTerm) ||
                x.ItemNo.Contains(filter.SearchTerm) || x.ItemName.Contains(filter.SearchTerm) || x.SafetyStockPharmacyChainGroup.Contains(filter.SearchTerm) || x.CheckDays.ToString().Contains(filter.SearchTerm) ||
                x.CheckDays.ToString().Contains(filter.SearchTerm) || x.WholesaleQuantity.ToString().Contains(filter.SearchTerm) || x.RetailQuantity.ToString().Contains(filter.SearchTerm) ||
                x.QuantityToBuy.ToString().Contains(filter.SearchTerm) || x.ItemGroup.Contains(filter.SearchTerm) || x.ProductClass.Contains(filter.SearchTerm) || x.Brand.Contains(filter.SearchTerm) ||
                x.SupplierNumber.ToString().Contains(filter.SearchTerm) || x.Cn3.Contains(filter.SearchTerm) || x.Cn1.Contains(filter.SearchTerm) || x.SupplierNick.Contains(filter.SearchTerm) ||
                x.Substance.Contains(filter.SearchTerm) || x.Comment.Contains(filter.SearchTerm));
            }
        }
    }
}
