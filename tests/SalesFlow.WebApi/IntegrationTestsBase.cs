using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesFlow.Communication.Request.User;
using SalesFlow.Communication.Response.User;
using SalesFlow.Infrastructure.DataAccess;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SalesFlow.WebApi;
public class IntegrationTestsBase : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    protected readonly WebApplicationFactory<Program> _factory;
    protected readonly HttpClient _client;
    protected readonly JsonSerializerOptions _jsonOptions;
    private readonly ApplicationDbContext _dbContext;
    private readonly string _dbName;

    public IntegrationTestsBase(WebApplicationFactory<Program> factory)
    {
        _dbName = $"TestingDB_{Guid.NewGuid()}";
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (dbContextDescriptor != null)
                    services.Remove(dbContextDescriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                });
            });
        });

        _client = _factory.CreateClient();
        var scope = _factory.Services.CreateScope();

        _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        _dbContext.Database.EnsureCreated();
    }

    protected StringContent GetStringContent(object obj) => new(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

    protected async Task<string> GetAuthToken()
    {
        var registerRequest = new RequestRegisterUserJson
        {
            Name = "Test User",
            Email = "test@test.com",
            Password = "Test@123",
            PasswordConfirm = "Test@123"
        };

        var registerResponse = await _client.PostAsync("/api/user/register", GetStringContent(registerRequest));
        var registerContent = await registerResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<ResponseUserJson>(registerContent, _jsonOptions);

        return userResponse?.Token ?? string.Empty;
    }

    protected async Task AuthenticateClient()
    {
        var token = await GetAuthToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
        _client.Dispose();
    }
}