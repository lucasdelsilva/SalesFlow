using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Communication.Response;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Exception;

namespace SalesFlow.Api.Controllers;

/// <summary>
/// Controlador para gerenciamento de vendas
/// </summary>
[Route("api/sales")]
[ApiController]
[Authorize]
[Produces("application/json")]
[Tags("Vendas")]
public class SalesController : ControllerBase
{
    /// <summary>
    /// Cria uma nova venda
    /// </summary>
    /// <param name="useCase">Use case de criação de venda</param>
    /// <param name="request">Dados da venda a ser criada</param>
    /// <returns>Dados da venda criada</returns>
    /// <response code="201">Venda criada com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="401">Não autorizado</response>
    [HttpPost]
    [ProducesResponseType(typeof(ResponseSaleJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Created(
        [FromServices] ISaleCreateUseCase useCase,
        [FromBody] RequestSaleCreateJson request)
    {
        var response = await useCase.Create(request);
        return Created(string.Empty, response);
    }

    /// <summary>
    /// Retorna todas as vendas cadastradas
    /// </summary>
    /// <param name="useCase">Use case de listagem de vendas</param>
    /// <returns>Lista de vendas</returns>
    /// <response code="200">Lista de vendas recuperada com sucesso</response>
    /// <response code="204">Nenhuma venda encontrada</response>
    /// <response code="401">Não autorizado</response>
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

    /// <summary>
    /// Retorna uma venda específica pelo ID
    /// </summary>
    /// <param name="useCase">Use case de busca de venda</param>
    /// <param name="id">ID da venda</param>
    /// <returns>Dados da venda</returns>
    /// <response code="200">Venda encontrada com sucesso</response>
    /// <response code="404">Venda não encontrada</response>
    /// <response code="401">Não autorizado</response>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseSaleJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromServices] ISaleGetByIdUseCase useCase,
        [FromRoute] long id)
    {
        var response = await useCase.GetById(id);
        if (response is not null)
            return Ok(response);
        return NotFound(ResourceErrorMessages.SALE_NOT_FOUND);
    }

    /// <summary>
    /// Remove uma venda específica
    /// </summary>
    /// <param name="useCase">Use case de remoção de venda</param>
    /// <param name="id">ID da venda a ser removida</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Venda removida com sucesso</response>
    /// <response code="404">Venda não encontrada</response>
    /// <response code="401">Não autorizado</response>
    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseSaleJson), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteById(
        [FromServices] ISaleDeleteUseCase useCase,
        [FromRoute] long id)
    {
        await useCase.Delete(id);
        return NoContent();
    }

    /// <summary>
    /// Atualiza uma venda e seus itens
    /// </summary>
    /// <param name="useCase">Use case de atualização de venda</param>
    /// <param name="id">ID da venda</param>
    /// <param name="request">Dados atualizados da venda</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Venda atualizada com sucesso</response>
    /// <response code="404">Venda não encontrada</response>
    /// <response code="401">Não autorizado</response>
    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseSaleJson), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleAndItem(
        [FromServices] ISaleUpdateUseCase useCase,
        [FromRoute] long id,
        [FromBody] RequestSaleUpdateJson request)
    {
        await useCase.Update(id, request);
        return NoContent();
    }

    /// <summary>
    /// Atualiza um item específico de uma venda
    /// </summary>
    /// <param name="useCase">Use case de atualização de item</param>
    /// <param name="id">ID da venda</param>
    /// <param name="itemId">ID do item</param>
    /// <param name="request">Dados atualizados do item</param>
    /// <returns>Sem conteúdo</returns>
    /// <response code="204">Item atualizado com sucesso</response>
    /// <response code="404">Venda ou item não encontrado</response>
    /// <response code="401">Não autorizado</response>
    [HttpPut("{id}/items/{itemId}")]
    [ProducesResponseType(typeof(ResponseSaleJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleItem(
        [FromServices] ISaleItemUpdateUseCase useCase,
        [FromRoute] long id,
        [FromRoute] long itemId,
        [FromBody] RequestSaleItemUpdateJson request)
    {
        request.Id = itemId;
        await useCase.UpdateItem(id, request);
        return NoContent();
    }
}