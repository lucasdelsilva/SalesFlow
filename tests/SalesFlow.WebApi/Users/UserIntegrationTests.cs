using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SalesFlow.Communication.Request.User;
using SalesFlow.Communication.Response.User;
using System.Text.Json;

namespace SalesFlow.WebApi.Tests.Users;
public class UserIntegrationTests : IntegrationTestsBase
{
    public UserIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task Register_WhenValidData_ShouldReturnSuccess()
    {
        // Arrange
        var request = new RequestRegisterUserJson
        {
            Name = "New User",
            Email = "newuser@test.com",
            Password = "Test@123",
            PasswordConfirm = "Test@123"
        };

        // Act
        var response = await _client.PostAsync("/api/user/register", GetStringContent(request));
        var content = await response.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<ResponseUserJson>(content, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        userResponse.Should().NotBeNull();
        userResponse!.Name.Should().Be(request.Name);
        userResponse.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WhenValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var email = "login@test.com";
        var password = "Test@123";

        var registerRequest = new RequestRegisterUserJson
        {
            Name = "Login Test",
            Email = email,
            Password = password,
            PasswordConfirm = password
        };

        await _client.PostAsync("/api/user/register", GetStringContent(registerRequest));

        var loginRequest = new RequestLoginUserJson
        {
            Email = email,
            Password = password
        };

        // Act
        var response = await _client.PostAsync("/api/user/login", GetStringContent(loginRequest));
        var content = await response.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<ResponseUserJson>(content, _jsonOptions);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        userResponse.Should().NotBeNull();
        userResponse!.Token.Should().NotBeNullOrEmpty();
    }
}