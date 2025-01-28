using Microsoft.AspNetCore.Mvc;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Communication.Response;
using SalesFlow.Communication.Response.Sales;

namespace SalesFlow.Api.Controllers;
[Route("api/sales")]
[ApiController]
public class SalesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseSaleCreateJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Created([FromServices] ISaleCreateUseCase useCase, [FromBody] RequestSaleCreateOrUpdateJson request)
    {
        var response = await useCase.Create(request);
        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseSalesJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Created([FromServices] ISaleGetAllUseCase useCase)
    {
        var response = await useCase.GetAll();
        if (response is not null && response.Sales.Count > 0)
            return Ok(response);

        return NoContent();
    }
}