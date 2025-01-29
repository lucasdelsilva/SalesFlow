using SalesFlow.Communication.Request.Sales;

namespace SalesFlow.Application.UseCases.Sales.Interfaces;
public interface ISaleItemUpdateUseCase
{
    Task UpdateItem(long id, RequestSaleItemUpdateJson request);
}
