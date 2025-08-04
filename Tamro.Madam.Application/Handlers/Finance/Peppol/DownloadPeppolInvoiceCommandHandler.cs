using MediatR;
using Microsoft.Extensions.Options;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Finance.Peppol;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Common.Configuration;
using TamroUtilities.MinIO;
using TamroUtilities.MinIO.Models;

namespace Tamro.Madam.Application.Handlers.Finance.Peppol;

[RequiresPermission(Permissions.CanViewPeppol)]
public class DownloadPeppolInvoiceCommandHandler : IRequestHandler<DownloadPeppolInvoiceCommand, Result<byte[]>>
{
    public readonly IFileStorage _fileStorage;
    public readonly MinioSettings _minioSettings;

    public DownloadPeppolInvoiceCommandHandler(IFileStorage fileStorage, IOptions<MinioSettings> minioSettings)
    {
        _fileStorage = fileStorage;
        _minioSettings = minioSettings.Value;
    }

    public async Task<Result<byte[]>> Handle(DownloadPeppolInvoiceCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _fileStorage.Get(new BaseFileRequest()
            {
                BucketName = _minioSettings.PeppolInvoicesBucketName,
                Path = $"LV/{command.InvoiceNumber}.xml"
            });

            return Result<byte[]>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<byte[]>.Failure(ex.Message);
        }
    }
}
