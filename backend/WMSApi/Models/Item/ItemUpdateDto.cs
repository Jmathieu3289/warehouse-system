namespace WMSApi.Models;

public class ItemUpdateDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string UPC { get; set; }
}
