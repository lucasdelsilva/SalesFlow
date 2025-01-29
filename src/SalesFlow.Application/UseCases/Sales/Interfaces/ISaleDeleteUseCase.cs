namespace SalesFlow.Application.UseCases.Sales.Interfaces;
public interface ISaleDeleteUseCase
{
    Task Delete(long id);
}