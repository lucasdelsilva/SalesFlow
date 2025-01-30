using AutoMapper;
using Moq;
using SalesFlow.Application.UseCases.User;
using SalesFlow.Communication.Request.User;
using SalesFlow.Domain.Entities;
using SalesFlow.Domain.Repositories.Interfaces;
using SalesFlow.Domain.Repositories.User;
using SalesFlow.Domain.Security.Cryptography;
using SalesFlow.Domain.Security.Tokens;

namespace CommonTests.Fixtures.Users;
public class RegisterUserTestFixture : IDisposable
{
    public Mock<IMapper> MapperMock { get; private set; }
    public Mock<IPasswordEncripter> PasswordEncripterMock { get; private set; }
    public Mock<IUserReadOnlyRepository> UserReadOnlyRepositoryMock { get; private set; }
    public Mock<IUserWriteOnlyRepository> UserWriteOnlyRepositoryMock { get; private set; }
    public Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }
    public Mock<IAccessTokenGenerator> AccessTokenGeneratorMock { get; private set; }
    public RegisterUserUseCase RegisterUserUseCase { get; private set; }

    private readonly string _validPassword = "Test@123";
    private readonly string _validEmail = "test@test.com";
    private readonly string _validName = "Test User";
    private readonly string _defaultToken = "token_test";

    public RegisterUserTestFixture()
    {
        MapperMock = new Mock<IMapper>();
        PasswordEncripterMock = new Mock<IPasswordEncripter>();
        UserReadOnlyRepositoryMock = new Mock<IUserReadOnlyRepository>();
        UserWriteOnlyRepositoryMock = new Mock<IUserWriteOnlyRepository>();
        UnitOfWorkMock = new Mock<IUnitOfWork>();
        AccessTokenGeneratorMock = new Mock<IAccessTokenGenerator>();

        RegisterUserUseCase = new RegisterUserUseCase(
            MapperMock.Object,
            PasswordEncripterMock.Object,
            UserReadOnlyRepositoryMock.Object,
            UserWriteOnlyRepositoryMock.Object,
            UnitOfWorkMock.Object,
            AccessTokenGeneratorMock.Object
        );
    }

    public RequestRegisterUserJson GetValidRegisterRequest()
    {
        return new RequestRegisterUserJson
        {
            Name = _validName,
            Email = _validEmail,
            Password = _validPassword,
            PasswordConfirm = _validPassword
        };
    }

    public User GetUserEntity()
    {
        return new User
        {
            Id = 1,
            Name = _validName,
            Email = _validEmail,
            Password = "encrypted_password"
        };
    }

    public void SetupSuccessfulRegister(RequestRegisterUserJson request, User user)
    {
        UserReadOnlyRepositoryMock
            .Setup(x => x.ExistActiveUserWithEmail(request.Email))
            .ReturnsAsync(false);

        MapperMock
            .Setup(x => x.Map<User>(request))
            .Returns(user);

        PasswordEncripterMock
            .Setup(x => x.Encrypt(request.Password))
            .Returns("encrypted_password");

        AccessTokenGeneratorMock
            .Setup(x => x.GeneratorToken(user))
            .Returns(_defaultToken);
    }

    public void VerifyRegisterSuccess(User user)
    {
        UserWriteOnlyRepositoryMock.Verify(x => x.Add(user), Times.Once);
        UnitOfWorkMock.Verify(x => x.Commit(), Times.Once);
    }

    public void VerifyRegisterFailure()
    {
        UserWriteOnlyRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
        UnitOfWorkMock.Verify(x => x.Commit(), Times.Never);
    }

    public void ResetMocks()
    {
        MapperMock.Reset();
        PasswordEncripterMock.Reset();
        UserReadOnlyRepositoryMock.Reset();
        UserWriteOnlyRepositoryMock.Reset();
        UnitOfWorkMock.Reset();
        AccessTokenGeneratorMock.Reset();
    }

    public void Dispose()
    {
        ResetMocks();
    }
}