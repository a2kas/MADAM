using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.Finance.Peppol;
using Tamro.Madam.Repository.Entities.Finance.Peppol;

namespace Tamro.Madam.Application.Queries.Finance.Peppol;

public class PeppolInvoiceSpecification : Specification<PeppolInvoice>
{
    public PeppolInvoiceSpecification(PeppolInvoiceFilter filter)
    {
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.CustomerName.Contains(filter.SearchTerm) || x.InvoiceNumber.ToString().Contains(filter.SearchTerm));
        }

        if (filter.Filters == null)
        {
            return;
        }

        if (filter.Status?.Count > 0)
        {
            Query.Where(x => filter.Status.Contains(x.Status));
        }

        if (filter.Types != null && filter.Types.Any())
        {
            var selectedTypes = filter.Types
                .Select(Enum.Parse<PeppolInvoiceType>)
                .ToArray();

            Query.Where(x => selectedTypes.Contains(x.Type));
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.CustomerName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.CustomerName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.SellerName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.SellerName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.SellerRegNo)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.SellerRegNo);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.InvoiceNumber)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.InvoiceNumber);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.InvoiceDate)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.InvoiceDate);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.InvoiceDueDate)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.InvoiceDueDate);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.TotalInvoiceTaxAmount)))
            {
                Query.ApplyDecimalFilter(appliedFilter.Operator, appliedFilter.Value, x => x.TotalInvoiceTaxAmount);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.TotalInvoiceAmountWithoutTax)))
            {
                Query.ApplyDecimalFilter(appliedFilter.Operator, appliedFilter.Value, x => x.TotalInvoiceAmountWithoutTax);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.TotalInvoiceAmountWithTax)))
            {
                Query.ApplyDecimalFilter(appliedFilter.Operator, appliedFilter.Value, x => x.TotalInvoiceAmountWithTax);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.CreatedDate)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.CreatedDate);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(PeppolInvoice.ConsolidationNumber)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.ConsolidationNumber);
            }
        }
    }
}
