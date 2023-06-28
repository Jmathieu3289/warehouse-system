namespace WMSApi;

public class PalletBayContentsDto
{
    public string? Name { get; set; }
    public long PalletId { get; set; }
    public long PalletBayId { get; set; }
    public double Quantity { get; set;}
}
