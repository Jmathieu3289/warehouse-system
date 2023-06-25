using Microsoft.EntityFrameworkCore;

namespace WMSApi.Models;

public class SalesOrderItemContext : DbContext
{
    public SalesOrderItemContext(DbContextOptions<SalesOrderItemContext> options) : base(options) {}
    public DbSet<SalesOrderItem> SalesOrderItems { get; set; } = null!;
}
