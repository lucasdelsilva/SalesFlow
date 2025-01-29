using SalesFlow.Communication.Request.Sales;

namespace SalesFlow.Application.UseCases.Sales.Interfaces;
public interface ISaleUpdateUseCase
{
    Task Update(long id, RequestSaleUpdateJson request);
}
