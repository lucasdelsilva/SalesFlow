using SalesFlow.Communication.Response.Sales;

namespace SalesFlow.Application.UseCases.Sales.Interfaces;
public interface ISaleGetAllUseCase
{
    Task<ResponseSalesJson> GetAll();
}