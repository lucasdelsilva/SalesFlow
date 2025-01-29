using Microsoft.AspNetCore.Mvc;
using SalesFlow.Application.UseCases.User.Interface;
using SalesFlow.Communication.Request.User;
using SalesFlow.Communication.Response;
using SalesFlow.Communication.Response.User;

namespace SalesFlow.Api.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase, [FromBody] RequestRegisterUserJson request)
    {
        var response = await useCase.Register(request);
        return Created(string.Empty, response);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] ILoginUserUseCase useCase, [FromBody] RequestLoginUserJson request)
    {
        var response = await useCase.Login(request);
        return Ok(response);
    }
}