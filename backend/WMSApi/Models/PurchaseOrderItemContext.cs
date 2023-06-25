using Microsoft.EntityFrameworkCore;

namespace WMSApi.Models;

public class PurchaseOrderItemContext : DbContext
{
    public PurchaseOrderItemContext(DbContextOptions<PurchaseOrderItemContext> options) : base(options) {}
    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; } = null!;
}
