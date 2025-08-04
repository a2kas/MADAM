using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Administration.Configuration.Ubl;

namespace Tamro.Madam.Application.Commands.Administration.Configuration.Ubl;
public class UpsertUblApiKeyCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public UblApiKeyEditModel Model { get; set; }

    public UpsertUblApiKeyCommand(UblApiKeyEditModel model)
    {
        Model = model;
    }

    public string ErrorMessage { get; set; } = "Failed to save UnifiedPost API key";
}
