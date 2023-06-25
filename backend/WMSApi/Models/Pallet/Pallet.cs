namespace WMSApi.Models;

public class Pallet
{
    public long Id { get; set; }
    public long PalletBayId { get; set; }
    public required PalletBay PalletBay { get; set; }
}
