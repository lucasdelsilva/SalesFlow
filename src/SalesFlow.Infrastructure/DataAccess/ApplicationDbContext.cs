using Microsoft.EntityFrameworkCore;
using SalesFlow.Domain.Entities;

namespace SalesFlow.Infrastructure.DataAccess;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleItem> SaleItems { get; set; }
    public DbSet<User> Users { get; set; }
}