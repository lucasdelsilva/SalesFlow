using SalesFlow.Domain.Entities;

namespace SalesFlow.Domain.Security.Tokens;
public interface IAccessTokenGenerator
{
    string GeneratorToken(User user);
}