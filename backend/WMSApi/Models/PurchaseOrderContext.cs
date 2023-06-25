using Microsoft.EntityFrameworkCore;

namespace WMSApi.Models;

public class PurchaseOrderContext : DbContext
{
    public PurchaseOrderContext(DbContextOptions<PurchaseOrderContext> options) : base(options) {}
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; } = null!;
}
