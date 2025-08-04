using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.Suppliers;

namespace Tamro.Madam.Application.Commands.Suppliers.Contracts;
public class ExtractContractFileCommand : IRequest<Result<SupplierContractExtractionModel>>, IDefaultErrorMessage
{
    public SupplierContractModel Contract { get; set; }
    public Stream Stream { get; set; }
    public string FileName { get; set; }
    public string ErrorMessage { get; set; } = "Failed to extract contract";

    public ExtractContractFileCommand(SupplierContractModel contract, Stream stream, string fileName)
    {
        Contract = contract;
        Stream = stream;
        FileName = fileName;
    }
}
