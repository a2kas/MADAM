using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Hr.Jira.Administration;
public class SetAccountsIsActivatedCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public SetAccountsIsActivatedCommand(string[] id, bool isActivated)
    {
        Ids = id;
        IsActivated = isActivated;
        ErrorMessage = $"Failed to {(IsActivated ? "" : "de")}activate accounts";
    }

    public string[] Ids { get; }
    public bool IsActivated { get; }
    public string ErrorMessage { get; set; }
}
