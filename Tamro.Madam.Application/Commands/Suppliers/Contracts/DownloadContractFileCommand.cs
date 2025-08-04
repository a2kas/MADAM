using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Suppliers.Contracts;
public class DownloadContractFileCommand : IRequest<Result<byte[]>>, IDefaultErrorMessage
{
    public string Path { get; set; }
    public string ErrorMessage { get; set; } = "Failed to download contract";

    public DownloadContractFileCommand(string path)
    {
        Path = path;
    }
}
