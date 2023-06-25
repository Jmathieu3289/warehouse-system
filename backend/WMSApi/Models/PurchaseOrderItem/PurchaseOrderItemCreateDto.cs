namespace WMSApi.Models;

public class PurchaseOrderItemCreateDto
{
    public required long ItemId { get; set; }
    public required long PurchaseOrderId { get; set; }
    public required double PurchasedQuantity { get; set; }
    public double Weight { get; set; }
    public required double UnitPrice { get; set; }
    public double MarkupPrice { get; set; }
    public double MarginPrice { get; set; }
    public double FreightPrice { get; set; }
}
