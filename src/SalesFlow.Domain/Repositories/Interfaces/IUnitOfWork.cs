namespace SalesFlow.Domain.Repositories.Interfaces;
public interface IUnitOfWork
{
    Task Commit();
}