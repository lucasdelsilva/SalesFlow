using Moq;
using SalesFlow.Application.UseCases.User;
using SalesFlow.Communication.Request.User;
using SalesFlow.Domain.Entities;
using SalesFlow.Domain.Repositories.User;
using SalesFlow.Domain.Security.Cryptography;
using SalesFlow.Domain.Security.Tokens;

namespace CommonTests.Fixtures.Users;
public class LoginUserTestFixture : IDisposable
{
    public Mock<IUserReadOnlyRepository> UserReadOnlyRepositoryMock { get; private set; }
    public Mock<IPasswordEncripter> PasswordEncripterMock { get; private set; }
    public Mock<IAccessTokenGenerator> AccessTokenGeneratorMock { get; private set; }
    public LoginUserUseCase LoginUserUseCase { get; private set; }

    private readonly string _validPassword = "Test@123";
    private readonly string _validEmail = "test@test.com";
    private readonly string _validName = "Test User";
    private readonly string _defaultToken = "token_test";
    private readonly string _encryptedPassword = "encrypted_password";

    public LoginUserTestFixture()
    {
        UserReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        PasswordEncripterMock = new Mock<IPasswordEncripter>();
        AccessTokenGeneratorMock = new Mock<IAccessTokenGenerator>();

        LoginUserUseCase = new LoginUserUseCase(
            UserReadOnlyRepositoryMock.Object,
            PasswordEncripterMock.Object,
            AccessTokenGeneratorMock.Object
        );
    }

    public RequestLoginUserJson GetValidLoginRequest()
    {
        return new RequestLoginUserJson
        {
            Email = _validEmail,
            Password = _validPassword
        };
    }

    public User GetValidUser()
    {
        return new User
        {
            Id = 1,
            Name = _validName,
            Email = _validEmail,
            Password = _encryptedPassword
        };
    }

    public void SetupSuccessfulLogin(RequestLoginUserJson request, User user)
    {
        UserReadOnlyRepositoryMock
            .Setup(x => x.GetUserByEmail(request.Email))
            .ReturnsAsync(user);

        PasswordEncripterMock
            .Setup(x => x.VerificationPassword(request.Password, user.Password))
            .Returns(true);

        AccessTokenGeneratorMock
            .Setup(x => x.GeneratorToken(user))
            .Returns(_defaultToken);
    }

    public void VerifyLoginAttempt()
    {
        UserReadOnlyRepositoryMock.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.Once);
    }

    public void ResetMocks()
    {
        UserReadOnlyRepositoryMock.Reset();
        PasswordEncripterMock.Reset();
        AccessTokenGeneratorMock.Reset();
    }

    public void Dispose()
    {
        ResetMocks();
    }
}