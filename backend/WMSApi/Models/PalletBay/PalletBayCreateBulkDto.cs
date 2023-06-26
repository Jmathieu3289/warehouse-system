namespace WMSApi.Models;

public class PalletBayCreateBulkDto
{
    public required string Row { get; set;}
    public required int StartSection { get; set; } 
    public required int EndSection { get; set; }
    public required int StartFloor { get; set; }
    public required int EndFloor { get; set; }
}
