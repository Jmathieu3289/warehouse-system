using Microsoft.EntityFrameworkCore;

namespace WMSApi.Models;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}
    public DbSet<Item> Items { get; set; }
    public DbSet<Pallet> Pallets { get; set; }
    public DbSet<PalletBay> PalletBays { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
}
