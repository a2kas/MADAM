using System.Linq.Expressions;
using Ardalis.Specification;
using LinqKit;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.Sales.HeldOrders;
using Tamro.Madam.Repository.Entities.Sales.HeldOrders;
using Z.EntityFramework.Plus;

namespace Tamro.Madam.Application.Queries.Sales.HeldOrders;
public class HeldOrderSpecification : Specification<E1HeldOrder>
{
    public HeldOrderSpecification(HeldOrderFilter filter)
    {
        Query.Where(x => x.Country == filter.Country);

        if (filter.NotificationStatus?.Any() == true)
        {
            Expression<Func<E1HeldOrder, bool>> predicate = x => false;

            Expression<Func<E1HeldOrder, bool>> tempPredicate = null;

            foreach (var status in filter.NotificationStatus)
            {
                if (status == E1HeldNotificationStatusModel.Sent)
                {
                    tempPredicate = x => x.NotificationStatus == E1HeldNotificationStatusModel.Sent;
                }
                else if (status == E1HeldNotificationStatusModel.FailureSending)
                {
                    tempPredicate = x => x.NotificationStatus == E1HeldNotificationStatusModel.FailureSending;
                }
                else if (status == E1HeldNotificationStatusModel.WillNotBeSent)
                {
                    tempPredicate = x => x.NotificationStatus == E1HeldNotificationStatusModel.WillNotBeSent;
                }
                else if (status == E1HeldNotificationStatusModel.NotSent)
                {
                    tempPredicate = x => x.NotificationStatus == E1HeldNotificationStatusModel.NotSent;
                }

                if (tempPredicate != null)
                {
                    predicate = predicate.Or(tempPredicate);
                }
            }

            Query.Where(predicate);
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.E1ShipTo.ToString().Contains(filter.SearchTerm) || x.DocumentNo.ToString().Contains(filter.SearchTerm) || x.Email.Contains(filter.SearchTerm));
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

            if (appliedFilter.Column.PropertyName.Equals(nameof(E1HeldOrder.OrderDate)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.OrderDate);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(E1HeldOrder.E1ShipTo)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.E1ShipTo);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(E1HeldOrder.DocumentNo)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.DocumentNo);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(E1HeldOrder.Email)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Email);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(E1HeldOrder.NotificationSendDate)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.NotificationSendDate);
            }
        }
    }
}
