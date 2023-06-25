using Microsoft.EntityFrameworkCore;

namespace WMSApi.Models;

public class SalesOrderContext : DbContext
{
    public SalesOrderContext(DbContextOptions<SalesOrderContext> options) : base(options) {}
    public DbSet<SalesOrder> SalesOrders { get; set; } = null!;
}
