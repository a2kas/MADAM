using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Tamro.Madam.Application.Access;
using Tamro.Madam.Application.Commands.ItemMasterdata.Draft.NewProductOffers.Upsert;
using Tamro.Madam.Application.Infrastructure.Attributes;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Validation;
using Tamro.Madam.Common.Configuration;
using Tamro.Madam.Models.General;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.Repository.Entities.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.Repository.UnitOfWork;
using TamroUtilities.MinIO;

namespace Tamro.Madam.Application.Commands.ItemMasterdata.CategoryManagers.Upsert;

[RequiresPermission(Permissions.CanEditCoreMasterdata)]
public class UpsertNewProductOfferCommandHandler(
    IMadamUnitOfWork _uow,
    IMapper _mapper,
    IFileStorage _fileStorage,
    IHandlerValidator _validationService,
    IOptions<MinioSettings> _minioSettings)
    : IRequestHandler<UpsertNewProductOfferCommand, Result<UpsertNewProductOfferResult>>
{
    public async Task<Result<UpsertNewProductOfferResult>> Handle(UpsertNewProductOfferCommand request, CancellationToken cancellationToken)
    {
        await _validationService.Validate(request);

        var uploadedFileRef = await UploadFileToMinioAndConstructFullRef(request.File, request.Country, request.SupplierId);

        var entity = _mapper.Map<NewProductOffer>(request);
        entity!.FileReference = uploadedFileRef;

        var trackedEntity = await _uow.GetRepository<NewProductOffer>().UpsertAsync(entity);
        await _uow.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<UpsertNewProductOfferResult>(trackedEntity);
        response!.FileReference = uploadedFileRef;

        return Result<UpsertNewProductOfferResult>.Success(response);
    }

    private async Task<string> UploadFileToMinioAndConstructFullRef(
        FileWithName file,
        BalticCountry country,
        int supplierId)
    {
        var extension = Path.GetExtension(file.Name);
        var filePath = $"items/newproductoffers/offers/{country}/{supplierId}_{Guid.NewGuid()}{extension}";

        await _fileStorage.Create(new TamroUtilities.MinIO.Models.CreateStreamedFileRequest
        {
            FileStream = file.Stream,
            BucketName = _minioSettings.Value.MasterdataBucketName,
            Path = filePath,
            Country = country.ToString(),
        });

        return $"{_minioSettings.Value.ReferenceBaseUrl}/{_minioSettings.Value.MasterdataBucketName}/{filePath}";
    }
}
