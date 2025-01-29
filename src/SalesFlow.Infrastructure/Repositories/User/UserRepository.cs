using Microsoft.EntityFrameworkCore;
using SalesFlow.Domain.Repositories.User;
using SalesFlow.Infrastructure.DataAccess;

namespace SalesFlow.Infrastructure.Repositories.User;
internal class UserRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Domain.Entities.User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email.Equals(email.ToLower()));
    }

    public async Task<Domain.Entities.User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.ToLower().Equals(email.ToLower()));
    }
}