using Microsoft.EntityFrameworkCore;

namespace WMSApi.Models;

[Index(nameof(Row), nameof(Section), nameof(Floor), IsUnique = true)]
public class PalletBay
{
    public long Id { get; set; }
    public required string Row { get; set;}
    public required string Section { get; set; } 
    public required string Floor { get; set;}
    public ICollection<Pallet>? Pallets{ get; set; }
}
