namespace WMSApi.Models;

public class PurchaseItemDto
{
    public long PurchaseOrderItemId { get; set; }
    public string? Name { get; set; }
    public required double Quantity { get; set; }
    public required double UnitPrice { get; set; }
}
