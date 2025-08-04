using System.Linq.Expressions;
using Ardalis.Specification;
using LinqKit;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.Sales.CanceledOrderLines;
using Tamro.Madam.Repository.Entities.Sales.CanceledOrderLines;

namespace Tamro.Madam.Application.Queries.Sales.CanceledOrderLines;

public class CanceledOrderHeaderSpecification : Specification<E1CanceledOrderHeader>
{
    public CanceledOrderHeaderSpecification(CanceledOrderHeaderFilter filter)
    {
        Query.Where(x => x.Country == filter.Country);

        if (filter.EmailStatus?.Any() == true)
        {
            Expression<Func<E1CanceledOrderHeader, bool>> predicate = x => false;

            foreach (var status in filter.EmailStatus)
            {
                Expression<Func<E1CanceledOrderHeader, bool>> tempPredicate = null;

                if (status == CanceledOrderHeaderEmailStatus.Sent)
                {
                    tempPredicate = x => x.Lines.All(y => y.EmailStatus == CanceledOrderLineEmailStatus.Sent);
                }
                else if (status == CanceledOrderHeaderEmailStatus.PartiallySent)
                {
                    tempPredicate = x => x.Lines.Any(y => y.EmailStatus == CanceledOrderLineEmailStatus.Sent) &&
                                         x.Lines.Any(y => y.EmailStatus != CanceledOrderLineEmailStatus.Sent);
                }
                else if (status == CanceledOrderHeaderEmailStatus.FailureSending)
                {
                    tempPredicate = x => x.Lines.All(y => y.EmailStatus == CanceledOrderLineEmailStatus.FailureSending);
                }
                else if (status == CanceledOrderHeaderEmailStatus.WillNotBeSent)
                {
                    tempPredicate = x => x.Lines.All(y => y.EmailStatus == CanceledOrderLineEmailStatus.WillNotBeSent);
                }
                else if (status == CanceledOrderHeaderEmailStatus.NotSent)
                {
                    tempPredicate = x => x.Lines.Any(y => y.EmailStatus == CanceledOrderLineEmailStatus.NotSent);
                }

                if (tempPredicate != null)
                {
                    predicate = predicate.Or(tempPredicate);
                }
            }

            Query.Where(predicate);
        }
        if (filter.OrderDateFrom != null)
        {
            Query.Where(x => x.OrderDate >= filter.OrderDateFrom);
        }
        if (filter.OrderDateTo != null)
        {
            Query.Where(x => x.OrderDate <= filter.OrderDateTo);
        }

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

            if (appliedFilter.Column.PropertyName.Equals(nameof(E1CanceledOrderHeader.OrderDate)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.OrderDate);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(E1CanceledOrderHeader.E1ShipTo)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.E1ShipTo);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(E1CanceledOrderHeader.CustomerOrderNo)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.CustomerOrderNo);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(E1CanceledOrderHeader.DocumentNo)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.DocumentNo);
            }
        }
        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.CustomerOrderNo.Contains(filter.SearchTerm) || x.DocumentNo.Contains(filter.SearchTerm));
        }
    }
}
