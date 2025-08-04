using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tamro.Madam.Application.Commands.ItemMasterdata.Draft.NewProductOffers.Upsert;
using Tamro.Madam.Application.Models.Common;
using Tamro.Madam.Models.ItemMasterdata.Draft.NewProductOffers;
using Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.Draft.NewProductOffers;
using TamroUtilities.Host.Extensions;

namespace Tamro.Madam.RestApi.Controllers.ItemMasterdata.Draft.NewProductOffers;

[ApiController]
[Route("/v1/items/offers")]
[SwaggerResponse(500, "Internal Server Error")]
[SwaggerResponse(400, "Bad Request (parameter out of bounds)")]
public class NewProductOfferController(
    IMediator _mediator,
    IMapper _mapper
)
: ControllerBaseTamro(_mapper)
{
    [HttpPost]
    [SwaggerResponse(200, "Offer created", typeof(Result<UpsertNewProductOfferResult>))]
    public async Task<IActionResult> CreateOffer([FromForm] CreateNewProductOfferViewModel request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = _mapper.Map<UpsertNewProductOfferCommand>(request);
        var response = await _mediator.Send(command);

        return Ok<Result<CreateNewProductOfferResultViewModel>>(response);
    }
}
