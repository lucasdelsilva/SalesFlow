using Microsoft.Extensions.DependencyInjection;
using SalesFlow.Application.AutoMapper;
using SalesFlow.Application.UseCases.Sales;
using SalesFlow.Application.UseCases.Sales.Interfaces;

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
    }
}