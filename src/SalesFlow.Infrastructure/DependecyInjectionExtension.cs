using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesFlow.Domain.Repositories.Interfaces;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Domain.Repositories.User;
using SalesFlow.Domain.Security.Cryptography;
using SalesFlow.Domain.Security.Tokens;
using SalesFlow.Domain.Services.LoggedUser;
using SalesFlow.Infrastructure.DataAccess;
using SalesFlow.Infrastructure.Repositories.Sales;
using SalesFlow.Infrastructure.Repositories.User;
using SalesFlow.Infrastructure.Security.Tokens;
using SalesFlow.Infrastructure.Services.LoggedUser;

namespace SalesFlow.Infrastructure;
public static class DependecyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordEncripter, Security.BCrypt>();
        services.AddScoped<ILoggedUser, LoggedUser>();

        AddRepositories(services);
        AddToken(services, configuration);

        AddDbContext(services, configuration);
    }

    private static void AddRepositories(IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddScoped<IUnitOfWork, UnitOfWork>();
        serviceDescriptors.AddScoped<ISalesWriteOnlyRepository, SalesRepository>();
        serviceDescriptors.AddScoped<ISalesReadOnlyRepository, SalesRepository>();

        //User
        serviceDescriptors.AddScoped<IUserReadOnlyRepository, UserRepository>();
        serviceDescriptors.AddScoped<IUserWriteOnlyRepository, UserRepository>();
    }

    private static void AddToken(IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        var expirationMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");
        var signinKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        //JWT
        serviceDescriptors.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expirationMinutes, signinKey!));
    }

    private static void AddDbContext(IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        serviceDescriptors.AddDbContext<ApplicationDbContext>(options => options.UseMySql(connectionString, serverVersion));
    }
}
