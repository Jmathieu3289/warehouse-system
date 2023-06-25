using Microsoft.EntityFrameworkCore;

namespace WMSApi.Models;

public class PalletContext : DbContext
{
    public PalletContext(DbContextOptions<PalletContext> options) : base(options) {}
    public DbSet<Pallet> Pallets { get; set; } = null!;
}
