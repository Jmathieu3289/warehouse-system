namespace WMSApi.Models;

public class Item
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string UPC { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateLastModified { get; set; }
}