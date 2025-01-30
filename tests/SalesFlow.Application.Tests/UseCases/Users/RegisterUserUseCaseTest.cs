using CommonTests.Fixtures.Users;
using CommonTests.InlineDatas.Users;
using FluentAssertions;
using Moq;
using SalesFlow.Communication.Request.User;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Tests.Application.UseCases.User;

public class RegisterUserUseCaseTest : IClassFixture<RegisterUserTestFixture>
{
    private readonly RegisterUserTestFixture _fixture;

    public RegisterUserUseCaseTest(RegisterUserTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetMocks();
    }

    private RequestRegisterUserJson Request()
    {
        return new RequestRegisterUserJson
        {
            Name = "User01",
            Email = "user01@teste.com",
            Password = "!Password123",
            PasswordConfirm = "!Password123"
        };
    }

    [Fact]
    public async Task Register_WhenSuccessful_ShouldReturnUserResponse()
    {
        var request = _fixture.GetValidRegisterRequest();
        var userEntity = _fixture.GetUserEntity();
        _fixture.SetupSuccessfulRegister(request, userEntity);

        var response = await _fixture.RegisterUserUseCase.Register(request);

        response.Should().NotBeNull();
        response.Name.Should().Be(request.Name);
        response.Token.Should().NotBeEmpty();

        _fixture.VerifyRegisterSuccess(userEntity);
    }

    [Fact]
    public async Task Register_WhenEmailAlreadyRegistered_ShouldThrowException()
    {
        var request = _fixture.GetValidRegisterRequest();
        _fixture.UserReadOnlyRepositoryMock.Setup(x => x.ExistActiveUserWithEmail(request.Email)).ReturnsAsync(true);

        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _fixture.RegisterUserUseCase.Register(request));
        _fixture.VerifyRegisterFailure();
    }

    [Theory]
    [ClassData(typeof(RegisterInlineData))]
    public async Task Register_WhenInvalidData_ShouldThrowException(string password)
    {
        var request = Request();
        request.Password = password;
        request.PasswordConfirm = password;

        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _fixture.RegisterUserUseCase.Register(request));
        _fixture.VerifyRegisterFailure();
    }

    [Fact]
    public async Task Register_WhenInvalidData_Name_Empty()
    {
        var request = Request();
        request.Name = string.Empty;

        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _fixture.RegisterUserUseCase.Register(request));
        _fixture.VerifyRegisterFailure();
    }


    [Fact]
    public async Task Register_WhenInvalidData_Email_Empty()
    {
        var request = Request();
        request.Email = string.Empty;

        await Assert.ThrowsAsync<ErrorOnValidationException>(() => _fixture.RegisterUserUseCase.Register(request));
        _fixture.VerifyRegisterFailure();
    }
}