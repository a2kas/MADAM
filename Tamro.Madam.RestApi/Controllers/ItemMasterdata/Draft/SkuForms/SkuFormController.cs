using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.Annotations;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Application.Queries.Items.Draft.SkuForms.ById;
using Tamro.Madam.Application.Queries.Items.Draft.SkuForms.Grid;
using Tamro.Madam.Application.Queries.Items.Draft.SkuForms.LatestForCountryAndType;
using Tamro.Madam.Common.Constants.Http;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.SkuForms;
using TamroUtilities.Host.Extensions;

namespace Tamro.Madam.RestApi.Controllers.ItemMasterdata.Draft.NewProductOffers;

[ApiController]
[Route("/v1/skuForms")]
[SwaggerResponse(500, "Internal Server Error")]
[SwaggerResponse(400, "Bad Request (parameter out of bounds)")]
public class SkuFormController(
    IMediator _mediator,
    IMapper _mapper
)
: ControllerBaseTamro(_mapper)
{
    [HttpGet]
    [SwaggerResponse(200, "SKU Forms fetched", typeof(Result<PaginatedData<SkuFormViewModel>>))]
    public async Task<IActionResult> SearchForSkuForms([FromQuery] SkuFormSearchViewModel query)
    {
        return Ok<PaginatedData<SkuFormViewModel>>(
            await _mediator.Send(_mapper.Map<SkuFormsGridQuery>(query)));
    }

    [HttpGet("{Id}")]
    [SwaggerResponse(200, "SKU Form fetched by id", typeof(IFileInfo))]
    [SwaggerResponse(404, "SKU Form not found", typeof(IFileInfo))]
    public async Task<IActionResult> GetSkuFormById(GetSkuFormByIdViewModel request)
    {
        var result = await _mediator.Send(_mapper.Map<SkuFormByIdQuery>(request));

        if (result.Data == null || !result.Succeeded)
        {
            return NotFound();
        }

        var fileStream = result.Data.Stream;
        var fileName = result.Data.Name;

        var contentType = MimeTypes.ExcelFile;
        return File(fileStream, contentType, fileName);
    }
    [HttpGet("{country}/latest")]
    [SwaggerResponse(200, "Latest SKU Form for Country and Type", typeof(IFileInfo))]
    [SwaggerResponse(404, "SKU Form not found for the specified Country and Type", typeof(IFileInfo))]
    public async Task<IActionResult> GetLatestSkuFormForCountryAndType(GetLatestSkuFormViewModel request)
    {
        var result = await _mediator.Send(_mapper.Map<SkuFormLatestForCountryAndTypeQuery>(request));

        if (result.Data == null || !result.Succeeded)
        {
            return NotFound();
        }

        var fileStream = result.Data.Stream;
        var fileName = result.Data.Name;

        var contentType = MimeTypes.ExcelFile;
        return File(fileStream, contentType, fileName);
    }
}
