using Microsoft.AspNetCore.Mvc;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Communication.Response;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Exception;

namespace SalesFlow.Api.Controllers;
[Route("api/sales")]
[ApiController]
public class SalesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseSaleJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Created([FromServices] ISaleCreateUseCase useCase, [FromBody] RequestSaleCreateJson request)
    {
        var response = await useCase.Create(request);
        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseSalesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] ISaleGetAllUseCase useCase)
    {
        var response = await useCase.GetAll();
        if (response is not null && response.Sales.Count > 0)
            return Ok(response);

        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseSaleJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] ISaleGetByIdUseCase useCase, [FromRoute] long id)
    {
        var response = await useCase.GetById(id);
        if (response is not null)
            return Ok(response);

        return NotFound(ResourceErrorMessages.SALE_NOT_FOUND);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseSaleJson), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleAndItem([FromServices] ISaleUpdateUseCase useCase, [FromRoute] long id, [FromBody] RequestSaleUpdateJson request)
    {
        await useCase.Update(id, request);
        return NoContent();
    }

    [HttpPut("{id}/items/{itemId}")]
    [ProducesResponseType(typeof(ResponseSaleJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleItem([FromServices] ISaleItemUpdateUseCase useCase, [FromRoute] long id, [FromRoute] long itemId, [FromBody] RequestSaleItemUpdateJson request)
    {
        request.Id = itemId;
        await useCase.UpdateItem(id, request);
        return NoContent();
    }
}