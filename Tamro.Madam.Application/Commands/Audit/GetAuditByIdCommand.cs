using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Audit;

namespace Tamro.Madam.Application.Commands.Audit;

public class GetAuditByIdCommand : IRequest<Result<AuditDetailsModel>>, IDefaultErrorMessage
{
    public GetAuditByIdCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
    public string ErrorMessage { get; set; } = "Failed to retrieve audit";
}
