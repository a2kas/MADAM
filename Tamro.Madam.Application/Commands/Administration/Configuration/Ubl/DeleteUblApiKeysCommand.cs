using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Administration.Configuration.Ubl;
public class DeleteUblApiKeysCommand : IRequest<Result<int>>, IDefaultErrorMessage
{
    public DeleteUblApiKeysCommand(int[] e1SoldTos)
    {
        E1SoldTos = e1SoldTos;
    }

    public int[] E1SoldTos { get; }
    public string ErrorMessage { get; set; } = "Failed to delete UnifiedPost API keys";
}
