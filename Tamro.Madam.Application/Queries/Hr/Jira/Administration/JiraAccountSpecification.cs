using Ardalis.Specification;
using Tamro.Madam.Application.Extensions.Specification;
using Tamro.Madam.Application.Utilities;
using Tamro.Madam.Models.Hr.Jira.Administration;
using Tamro.Madam.Repository.Entities.Jpg;

namespace Tamro.Madam.Application.Queries.Hr.Jira.Administration;
public class JiraAccountSpecification : Specification<JiraAccount>
{
    public JiraAccountSpecification(JiraAccountFilter filter)
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

            if (appliedFilter.Column.PropertyName.Equals(nameof(JiraAccountModel.DisplayName)))
            {
                Query.ApplyStringFilter(appliedFilter.Operator, (string)appliedFilter.Value, x => x.DisplayName);
            }
            if (appliedFilter.Column.PropertyName.Equals(nameof(JiraAccountModel.IsActive)))
            {
                Query.ApplyBoolFilter((bool?)appliedFilter.Value, x => x.IsActive);
            }
        }

        if (filter.Teams != null && filter.Teams.Any())
        {
            var selectedTeams = filter.Teams
                .Select(EnumParser.ParseNullable<JiraAccountTeam>)
                .ToArray();

            Query.Where(e => selectedTeams.Contains(e.Team));
        }

        if (filter.IsActiveFilter != null && filter.IsActiveFilter.Any())
        {
            var selectedBoolValues = filter.IsActiveFilter
                    .Select(bool.Parse)
                    .ToArray();
            Query.Where(e => selectedBoolValues.Contains(e.IsActive));
        }
    }
}