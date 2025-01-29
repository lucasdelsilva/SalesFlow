using SalesFlow.Application.UseCases.User.Interface;
using SalesFlow.Communication.Request.User;
using SalesFlow.Communication.Response.User;
using SalesFlow.Domain.Repositories.User;
using SalesFlow.Domain.Security.Cryptography;
using SalesFlow.Domain.Security.Tokens;
using SalesFlow.Exception;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.UseCases.User;
public class LoginUserUseCase : ILoginUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;

    public LoginUserUseCase(IUserReadOnlyRepository userReadOnlyRepository, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<ResponseUserJson> Login(RequestLoginUserJson request)
    {
        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);
        if (user is null)
            throw new InvalidLoginException(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);

        var passwordMatch = _passwordEncripter.VerificationPassword(request.Password, user.Password);
        if (!passwordMatch)
            throw new InvalidLoginException(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);

        return new ResponseUserJson
        {
            Name = user.Name,
            Token = _accessTokenGenerator.GeneratorToken(user)
        };
    }
}