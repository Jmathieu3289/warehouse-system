using Microsoft.EntityFrameworkCore;

namespace WMSApi.Models;

public class PalletBayContext : DbContext
{
    public PalletBayContext(DbContextOptions<PalletBayContext> options) : base(options) {}
    public DbSet<PalletBay> PalletBays { get; set; } = null!;
}
