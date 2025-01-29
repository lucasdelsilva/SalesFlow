using SalesFlow.Communication.Response.Sales;

namespace SalesFlow.Application.UseCases.Sales.Interfaces;
public interface ISaleGetByIdUseCase
{
    Task<ResponseSaleJson?> GetById(long id);
}