using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Items.SafetyStocks.PharmacyChains;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.SafetyStocks;

namespace Tamro.Madam.Application.Queries.Items.SafetyStocks.PharmacyChains;

public class PharmacyChainSpecification : Specification<SafetyStockPharmacyChain>
{
    public PharmacyChainSpecification(PharmacyChainFilter filter)
    {
        if (filter.Filters == null)
        {
            return;
        }
        if (filter.Country != null)
        {
            Query.Where(x => x.Country == filter.Country);
        }
        if (filter.Group?.Any() == true)
        {
            var selectedGroups = filter.Group
                .Select(Enum.Parse<PharmacyGroup>)
                .ToArray();
            Query.Where(x => selectedGroups.Contains(x.Group));
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PharmacyChainModel.DisplayName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.DisplayName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PharmacyChainModel.IsActive)) && appliedFilter.Value != null)
            {
                Query.ApplyBoolFilter((bool)appliedFilter.Value, x => x.IsActive);
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
            else if (Enum.GetNames(typeof(PharmacyGroup)).Any(c => c.ToLower().Contains(searchTermLower)))
            {
                var matchingGroups = Enum.GetValues(typeof(PharmacyGroup)).Cast<PharmacyGroup>().Where(c => c.ToString().ToLower().Contains(searchTermLower));

                Query.Where(x => matchingGroups.Contains(x.Group));
            }
            else
            {
                Query.Where(x => x.DisplayName.Contains(filter.SearchTerm));
            }
        }
    }
}
