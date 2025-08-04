using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.General;

namespace Tamro.Madam.Application.Commands.Suppliers.Contracts;

public class UploadContractFileCommand : IRequest<Result<string>>, IDefaultErrorMessage
{
    public BalticCountry Country { get; set; }
    public string SupplierRegistrationNumber { get; set; }
    public Stream FileStream { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public string ErrorMessage { get; set; } = "File upload failed.";

    public UploadContractFileCommand(BalticCountry country, string supplierRegistrationNumber, Stream fileContent, string fileName, string fileExtension)
    {
        Country = country;
        SupplierRegistrationNumber = supplierRegistrationNumber;
        FileStream = fileContent;
        FileName = fileName;
        FileExtension = fileExtension;
    }
}
