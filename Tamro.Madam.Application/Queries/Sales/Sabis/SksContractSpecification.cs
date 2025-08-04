using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.Sales.Sabis;
using Tamro.Madam.Repository.Entities.Sales.Sabis;

namespace Tamro.Madam.Application.Queries.Sales.Sabis;

public class SksContractSpecification : Specification<SksContractMapping>
{
    public SksContractSpecification(SksContractFilter filter)
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

            if (appliedFilter.Column.PropertyName.Equals(nameof(SksContractGridModel.CustomerName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Customer.MailingName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SksContractGridModel.CompanyId)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.AdditionalTaxId);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SksContractGridModel.AddressNumber)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.AddressNumber);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SksContractGridModel.ContractTamro)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ContractTamro);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SksContractGridModel.ContractSabis)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ContractSabis);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SksContractGridModel.EditedAt)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.RowVer);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(SksContractGridModel.EditedBy)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.EditedBy);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.AdditionalTaxId.Contains(filter.SearchTerm) ||
                x.AddressNumber.ToString().Contains(filter.SearchTerm) ||
                x.Customer.MailingName.Contains(filter.SearchTerm) ||
                x.ContractTamro.Contains(filter.SearchTerm) ||
                x.ContractSabis.Contains(filter.SearchTerm) ||
                x.EditedBy.Contains(filter.SearchTerm));
        }
    }
}
