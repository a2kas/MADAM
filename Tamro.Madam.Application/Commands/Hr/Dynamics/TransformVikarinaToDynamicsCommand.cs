using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Hr.Dynamics;

public class TransformVikarinaToDynamicsCommand : IRequest<Result<byte[]>>, IDefaultErrorMessage
{
    public TransformVikarinaToDynamicsCommand(List<byte[]> vikarinaFiles)
    {
        VikarinaFiles = vikarinaFiles;
    }

    public List<byte[]> VikarinaFiles { get; }
    public string ErrorMessage { get; set; } = "Failed to transform";
}
