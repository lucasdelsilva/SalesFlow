using Microsoft.EntityFrameworkCore;
using SalesFlow.Domain.Entities;
using SalesFlow.Domain.Security.Tokens;
using SalesFlow.Domain.Services.LoggedUser;
using SalesFlow.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SalesFlow.Infrastructure.Services.LoggedUser;
internal class LoggedUser : ILoggedUser
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(ApplicationDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;

    }
    public async Task<User> Get()
    {
        string token = _tokenProvider.TokenOnRequest();
        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type.Equals(ClaimTypes.Sid)).Value;

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.UserId.Equals(Guid.Parse(identifier)));
    }
}