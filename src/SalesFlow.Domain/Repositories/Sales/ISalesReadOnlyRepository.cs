using SalesFlow.Domain.Entities;

namespace SalesFlow.Domain.Repositories.Sales;
public interface ISalesReadOnlyRepository
{
    Task<List<Sale>> GetAll(Entities.User user);
    Task<Sale?> GetById(Entities.User user, long id);
    Task<Sale?> UpdateOrRemoveGetById(Entities.User user, long id);
}