using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Queries.Items.Draft.SkuForms.LatestForCountryAndType;
using Tamro.Madam.Common.Configuration;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.SkuForm;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.SkuForms;
using Tamro.Madam.Repository.UnitOfWork;
using TamroUtilities.MinIO;
using TamroUtilities.MinIO.Models;

namespace Tamro.Madam.Application.Queries.Items.Draft.SkuForms.LatestForCountry;
[RequiresPermission(Permissions.CanViewProductOffers)]
public class SkuFormLatestForCountryAndTypeQueryHandler(
    IMadamUnitOfWork _uow,
    IFileStorage _fileStorage,
    IOptions<MinioSettings> _minioSettings
) : IRequestHandler<SkuFormLatestForCountryAndTypeQuery, Result<FileWithName?>>
{
    public async Task<Result<FileWithName?>> Handle(SkuFormLatestForCountryAndTypeQuery request, CancellationToken cancellationToken)
    {
        var repo = _uow.GetRepository<SkuForm>();

        var foundSkuForm = await repo
            .AsReadOnlyQueryable()
            .Where(sf => sf.Country == request.Country)
            .Where(sf => sf.Type == request.Type)
            .OrderByDescending(sf => sf.VersionMajor)
            .ThenBy(sf => sf.VersionMinor)
            .FirstOrDefaultAsync();

        if (foundSkuForm == null)
        {
            return Result<FileWithName?>.Success(null);
        }

        var fileData = await _fileStorage.Get(new BaseFileRequest
        {
            BucketName = _minioSettings.Value.MasterdataBucketName,
            Path = foundSkuForm.FileReference,
        });

        var resultFileInfo = new FileWithName
        {
            Name = SkuFormModel.ConstructFullSkuFormFileName(
                foundSkuForm.Country,
                foundSkuForm.Type,
                foundSkuForm.VersionMajor,
                foundSkuForm.VersionMinor,
                foundSkuForm.FileReference),
            Stream = new MemoryStream(fileData),
        };

        return Result<FileWithName?>.Success(resultFileInfo);
    }
}
