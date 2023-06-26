namespace WMSApi.Models;

public class PalletBayCreateDto
{
    public required string Row { get; set;}
    public required string Section { get; set; } 
    public required string Floor { get; set;}
}
