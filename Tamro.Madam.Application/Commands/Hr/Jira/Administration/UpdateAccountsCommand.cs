using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Hr.Jira.Administration;

namespace Tamro.Madam.Application.Commands.Hr.Jira.Administration;
public class UpdateAccountsCommand : IRequest<Result<JiraAccountModel>>, IDefaultErrorMessage
{
    public UpdateAccountsCommand(JiraAccountModel model)
    {
        Model = model;
    }

    public JiraAccountModel Model { get; set; }
    public string ErrorMessage { get; set; } = "Failed to save Jira account";
}
