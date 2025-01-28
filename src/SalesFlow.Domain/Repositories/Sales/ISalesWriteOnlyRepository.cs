using SalesFlow.Domain.Entities;

namespace SalesFlow.Domain.Repositories.Sales;
public interface ISalesWriteOnlyRepository
{
    Task Create(Sale sale);
    Task Delete(long id);
    void Update(Sale request);
}