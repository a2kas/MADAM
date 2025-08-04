using System.Linq.Expressions;
using Ardalis.Specification;
using LinqKit;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Models.ItemMasterdata.Items.QualityCheck;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Items.QualityCheck;

namespace Tamro.Madam.Application.Queries.Items.QualityCheck;

public class ItemQualityCheckSpecification : Specification<ItemQualityCheck>
{
    public ItemQualityCheckSpecification(ItemQualityCheckFilter filter)
    {
        if (filter.Filters == null)
        {
            return;
        }

        if (filter.UnresolvedSeverities?.Any() == true)
        {
            Expression<Func<ItemQualityCheck, bool>> predicate = x => false;

            foreach (var severity in filter.UnresolvedSeverities)
            {
                predicate = predicate.Or(x => x.ItemQualityCheckIssues.Any(y => y.IssueSeverity == severity && y.IssueStatus == ItemQualityIssueStatus.New));
            }

            Query.Where(predicate);
        }

        if (filter.Statuses?.Any() == true)
        {
            Expression<Func<ItemQualityCheck, bool>> predicate = x => false;

            foreach (var status in filter.Statuses)
            {
                predicate = predicate.Or(x => x.ItemQualityCheckIssues.Any(y => y.IssueStatus == status));
            }

            Query.Where(predicate);
        }

        foreach (var appliedFilter in filter.Filters)
        {
            if (appliedFilter == null || appliedFilter.Column == null)
            {
                continue;
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemQualityCheckGridModel.ItemId)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.ItemId);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemQualityCheckGridModel.ItemName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.Item.ItemName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemQualityCheckGridModel.IssueCount)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.ItemQualityCheckIssues.Count);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemQualityCheckGridModel.UnresolvedIssuesCount)))
            {
                Query.ApplyIntFilter(appliedFilter.Operator, appliedFilter.Value, x => x.ItemQualityCheckIssues.Count(x => x.IssueStatus == ItemQualityIssueStatus.New));
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(ItemQualityCheckGridModel.ScanDate)))
            {
                Query.ApplyDateTimeFilter(appliedFilter.Operator, (DateTime?)appliedFilter.Value, x => x.RowVer);
            }
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            Query.Where(x => x.Item.ItemName!.Contains(filter.SearchTerm) || x.ItemId.ToString().Contains(filter.SearchTerm));
        }
    }
}
