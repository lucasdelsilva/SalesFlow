using CommonTests.Fixtures.Users;
using FluentAssertions;
using Moq;
using SalesFlow.Communication.Request.User;
using SalesFlow.Exception.ExceptionBase;

namespace SalesFlow.Application.Tests.UseCases.Users;
public class LoginUserUseCaseTest : IClassFixture<LoginUserTestFixture>
{
    private readonly LoginUserTestFixture _fixture;

    public LoginUserUseCaseTest(LoginUserTestFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetMocks();
    }

    [Fact]
    public async Task Login_Success_ReturnUserResponse()
    {
        var request = _fixture.GetValidLoginRequest();
        var user = _fixture.GetValidUser();


        _fixture.SetupSuccessfulLogin(request, user);
        var response = await _fixture.LoginUserUseCase.Login(request);

        response.Should().NotBeNull();
        response.Name.Should().Be(user.Name);
        response.Token.Should().NotBeEmpty();

        _fixture.VerifyLoginAttempt();
    }

    [Fact]
    public async Task Login_WhenUserNotFound_Exception()
    {
        var request = _fixture.GetValidLoginRequest();
        _fixture.UserReadOnlyRepositoryMock.Setup(x => x.GetUserByEmail(request.Email)).ReturnsAsync((Domain.Entities.User)null!);

        var exception = await Assert.ThrowsAsync<InvalidLoginException>(() => _fixture.LoginUserUseCase.Login(request));
        exception.Message.Should().NotBeEmpty();

        _fixture.VerifyLoginAttempt();
    }

    [Fact]
    public async Task Login_WhenPasswordIsIncorrect_Exception()
    {
        var request = _fixture.GetValidLoginRequest();
        var user = _fixture.GetValidUser();

        _fixture.UserReadOnlyRepositoryMock.Setup(x => x.GetUserByEmail(request.Email)).ReturnsAsync(user);
        _fixture.PasswordEncripterMock.Setup(x => x.VerificationPassword(request.Password, user.Password)).Returns(false);

        var exception = await Assert.ThrowsAsync<InvalidLoginException>(() => _fixture.LoginUserUseCase.Login(request));
        exception.Message.Should().NotBeEmpty();

        _fixture.VerifyLoginAttempt();
    }

    [Theory]
    [InlineData(null, "!Password123")]
    [InlineData("user@test.com", null)]
    [InlineData("invalid_email", "!Password123")]
    public async Task Login_WhenInvalidData_Exception(string email, string password)
    {
        var request = new RequestLoginUserJson
        {
            Email = email,
            Password = password
        };

        await Assert.ThrowsAsync<InvalidLoginException>(() => _fixture.LoginUserUseCase.Login(request));
    }
}