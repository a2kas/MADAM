using MediatR;
using Tamro.Madam.Application.Infrastructure.Behaviors;
using Tamro.Madam.Application.Models.Common;

namespace Tamro.Madam.Application.Commands.Finance.Peppol;

public class DownloadPeppolInvoiceCommand : IRequest<Result<byte[]>>, IDefaultErrorMessage
{
    public string InvoiceNumber { get; set; }
    public string ErrorMessage { get; set; } = "Failed to download invoice";

    public DownloadPeppolInvoiceCommand(string invoiceNumber)
    {
        InvoiceNumber = invoiceNumber;
    }
}
