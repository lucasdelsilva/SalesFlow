using Microsoft.Extensions.DependencyInjection;
using SalesFlow.Application.AutoMapper;
using SalesFlow.Application.UseCases.Sales;
using SalesFlow.Application.UseCases.Sales.Interfaces;
using SalesFlow.Application.UseCases.User;
using SalesFlow.Application.UseCases.User.Interface;

namespace SalesFlow.Application;
public static class DependecyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMaper(services);
        AddUseCases(services);
    }

    public static void AddAutoMaper(IServiceCollection serviceDescriptors)
    {
        serviceDescriptors.AddAutoMapper(typeof(AutoMapping));
    }

    public static void AddUseCases(IServiceCollection serviceDescriptors)
    {
        //Sale
        serviceDescriptors.AddScoped<ISaleCreateUseCase, SaleCreateUseCase>();
        serviceDescriptors.AddScoped<ISaleGetAllUseCase, SaleGetAllUseCase>();
        serviceDescriptors.AddScoped<ISaleGetByIdUseCase, SaleGetByIdUseCase>();
        serviceDescriptors.AddScoped<ISaleUpdateUseCase, SaleUpdateUseCase>();
        serviceDescriptors.AddScoped<ISaleItemUpdateUseCase, SaleItemUpdateUseCase>();
        serviceDescriptors.AddScoped<ISaleDeleteUseCase, SaleDeleteUseCase>();

        //User
        serviceDescriptors.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        serviceDescriptors.AddScoped<ILoginUserUseCase, LoginUserUseCase>();
    }
}