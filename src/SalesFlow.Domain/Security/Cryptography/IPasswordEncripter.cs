namespace SalesFlow.Domain.Security.Cryptography;
public interface IPasswordEncripter
{
    string Encrypt(string password);
    bool VerificationPassword(string password, string passwordHash);
}