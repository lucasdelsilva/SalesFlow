
using SalesFlow.Domain.Entities;

namespace SalesFlow.Domain.Services.LoggedUser;
public interface ILoggedUser
{
    Task<User> Get();
}
