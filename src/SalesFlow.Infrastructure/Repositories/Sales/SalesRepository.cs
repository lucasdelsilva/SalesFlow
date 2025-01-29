using Microsoft.EntityFrameworkCore;
using SalesFlow.Domain.Entities;
using SalesFlow.Domain.Repositories.Sales;
using SalesFlow.Infrastructure.DataAccess;

namespace SalesFlow.Infrastructure.Repositories.Sales;
internal class SalesRepository : ISalesWriteOnlyRepository, ISalesReadOnlyRepository
{
    private readonly ApplicationDbContext _dbContext;
    public SalesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Create(Sale sale)
    {
        await _dbContext.Sales.AddAsync(sale);
    }

    public async Task Delete(long id)
    {
        var sale = await _dbContext.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id.Equals(id));
        if (sale is not null)
        {
            _dbContext.SaleItems.RemoveRange(sale.Items!);
            _dbContext.Sales.Remove(sale);
        }
    }

    public async Task<List<Sale>> GetAll(Domain.Entities.User user)
    {
        return await _dbContext.Sales.AsNoTracking().Where(s => s.UserId.Equals(user.Id)).OrderByDescending(x => x.Date).ToListAsync();
    }

    public async Task<Sale?> GetById(Domain.Entities.User user, long id)
    {
        return await _dbContext.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id.Equals(id) && s.UserId.Equals(user.Id));
    }

    public void Update(Sale request)
    {
        _dbContext.Sales.Update(request);
    }

    public async Task<Sale?> UpdateOrRemoveGetById(Domain.Entities.User user, long id)
    {
        return await _dbContext.Sales.Include(s => s.Items).FirstOrDefaultAsync(s => s.Id.Equals(id) && s.UserId.Equals(user.Id));
    }
}