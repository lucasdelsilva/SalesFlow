using SalesFlow.Communication.Request.Sales;
using SalesFlow.Communication.Response.Sales;

namespace SalesFlow.Application.UseCases.Sales.Interfaces;
public interface ISaleCreateUseCase
{
    Task<ResponseSaleJson> Create(RequestSaleCreateJson request);
}