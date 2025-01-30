using Microsoft.AspNetCore.Mvc;
using SalesFlow.Application.UseCases.User.Interface;
using SalesFlow.Communication.Request.User;
using SalesFlow.Communication.Response;
using SalesFlow.Communication.Response.User;

namespace SalesFlow.Api.Controllers;

/// <summary>
/// Controller responsável pelo gerenciamento de usuários
/// </summary>
[Route("api/user")]
[ApiController]
[Produces("application/json")]
[Tags("Usuários")]
public class UserController : ControllerBase
{
    /// <summary>
    /// Registra um novo usuário no sistema
    /// </summary>
    /// <param name="useCase">Use case de registro de usuário</param>
    /// <param name="request">Dados do usuário para registro</param>
    /// <response code="201">Usuário registrado com sucesso</response>
    /// <response code="400">Dados de registro inválidos</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Register(request);
        return Created(string.Empty, response);
    }

    /// <summary>
    /// Autentica um usuário no sistema
    /// </summary>
    /// <param name="useCase">Use case de login de usuário</param>
    /// <param name="request">Credenciais do usuário</param>
    /// <response code="200">Login realizado com sucesso</response>
    /// <response code="401">Credenciais inválidas</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] ILoginUserUseCase useCase,
        [FromBody] RequestLoginUserJson request)
    {
        var response = await useCase.Login(request);
        return Ok(response);
    }
}