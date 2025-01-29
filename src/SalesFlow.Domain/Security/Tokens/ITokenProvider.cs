namespace SalesFlow.Domain.Security.Tokens;
public interface ITokenProvider
{
    string TokenOnRequest();
}