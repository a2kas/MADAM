using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Models.Hr.Jira.Administration;
using Tamro.Madam.Repository.Common;

namespace Tamro.Madam.Application.Queries.Hr.Jira.Administration.Grid;
public class JiraAccountsGridQuery : JiraAccountFilter, IRequest<PaginatedData<JiraAccountModel>>, IDefaultErrorMessage
{
    public JiraAccountSpecification Specification => new(this);
    public string ErrorMessage { get; set; } = "Failed to fetch data for jira accounts grid";
}
