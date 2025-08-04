using MediatR;
using Microsoft.Extensions.Options;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.Suppliers.Contracts;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Common.Configuration;
using TamroUtilities.MinIO;
using TamroUtilities.MinIO.Models;

namespace Tamro.Madam.Application.Handlers.Suppliers.Contracts;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UploadContractFileCommandHandler : IRequestHandler<UploadContractFileCommand, Result<string>>
{
    public readonly IFileStorage _fileStorage;
    public readonly MinioSettings _minioSettings;

    public UploadContractFileCommandHandler(IFileStorage fileStorage, IOptions<MinioSettings> minioSettings)
    {
        _fileStorage = fileStorage;
        _minioSettings = minioSettings.Value;
    }

    public async Task<Result<string>> Handle(UploadContractFileCommand command, CancellationToken cancellationToken)
    {
        string path = CreatePath(command);
        try
        {
            await _fileStorage.Create(new CreateStreamedFileRequest
            {
                BucketName = _minioSettings.SupplierContractsBucketName,
                FileStream = command.FileStream,
                Path = path
            });
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
        return Result<string>.Success(path);
    }

    private static string CreatePath(UploadContractFileCommand command)
    {
        var folder = string.IsNullOrEmpty(command.SupplierRegistrationNumber) ? "tmp" : command.SupplierRegistrationNumber;

        return $"{command.Country.ToString().ToLower()}/{folder}/{DateTime.Now:yyyy_MM_dd_HH_mm_ss}_contract.{command.FileExtension}";
    }
}
