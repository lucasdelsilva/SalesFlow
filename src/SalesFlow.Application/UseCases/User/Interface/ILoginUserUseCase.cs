
using SalesFlow.Communication.Request.User;
using SalesFlow.Communication.Response.User;

namespace SalesFlow.Application.UseCases.User.Interface;
public interface ILoginUserUseCase
{
    Task<ResponseUserJson> Login(RequestLoginUserJson request);
}