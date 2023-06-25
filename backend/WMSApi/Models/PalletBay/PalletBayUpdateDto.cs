namespace WMSApi;

public class PalletBayUpdateDto
{
    public long Id { get; set; }
    public required string Row { get; set;}
    public required string Section { get; set; } 
    public required string Floor { get; set;}
}
