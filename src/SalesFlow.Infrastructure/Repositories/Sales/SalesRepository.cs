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
        var sale = await _dbContext.Sales.FindAsync(id);
        _dbContext.Sales.Remove(sale!);
    }

    public async Task<List<Sale>> GetAll()
    {
        return await _dbContext.Sales.AsNoTracking().OrderByDescending(x => x.Date).ToListAsync();
    }

    public void Update(Sale request)
    {
        _dbContext.Sales.Update(request);
    }
}
