using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Tamro.Madam.Application.Queries.CategoryManagers.Grid;
using Tamro.Madam.Repository.Common;
using Tamro.Madam.RestApi.Contracts.Models.ItemMasterdata.CategoryManagers.Index;
using TamroUtilities.Host.Extensions;

namespace Tamro.Madam.RestApi.Controllers.ItemMasterdata.CategoryManagers;

[ApiController]
[Route("/v1/[controller]")]
[SwaggerResponse(500, "Internal Server Error")]
[SwaggerResponse(400, "Bad Request (parameter out of bounds)")]
public class CategoryManagersController(
    IMediator _mediator,
    IMapper _mapper
)
: ControllerBaseTamro(_mapper)
{
    [HttpGet("", Name = "Category Managers listing")]
    [SwaggerResponse(200, "Category Managers listed", typeof(PaginatedData<CategoryManagerViewModel>))]
    public async Task<IActionResult> Get([FromQuery] CategoryManagerSearchViewModel query)
    {
        return Ok<PaginatedData<CategoryManagerViewModel>>(
            await _mediator.Send(_mapper.Map<CategoryManagersGridQuery>(query)));
    }
}
