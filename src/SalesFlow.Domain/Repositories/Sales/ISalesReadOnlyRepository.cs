using SalesFlow.Domain.Entities;

namespace SalesFlow.Domain.Repositories.Sales;
public interface ISalesReadOnlyRepository
{
    Task<List<Sale>> GetAll();
}
