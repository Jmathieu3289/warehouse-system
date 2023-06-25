namespace WMSApi.Models;

public class PurchaseOrderItemUpdateDto
{
    public long Id { get; set; }
    public long PalletId { get; set; }
    public double PurchasedQuantity { get; set; }
    public double Weight { get; set; }
    public double UnitPrice { get; set; }
    public double MarkupPrice { get; set; }
    public double MarginPrice { get; set; }
    public double FreightPrice { get; set; }
}
