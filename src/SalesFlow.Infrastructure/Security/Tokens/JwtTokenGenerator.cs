using Microsoft.IdentityModel.Tokens;
using SalesFlow.Domain.Entities;
using SalesFlow.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SalesFlow.Infrastructure.Security.Tokens;
public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signingKey;

    public JwtTokenGenerator(uint expirationTimeMinutes, string signingKey)
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signingKey = signingKey;
    }
    public string GeneratorToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Sid, user.UserId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);

    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signingKey);
        return new SymmetricSecurityKey(key);
    }
}
