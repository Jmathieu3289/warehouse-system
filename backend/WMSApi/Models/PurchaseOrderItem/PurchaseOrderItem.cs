namespace WMSApi.Models;

public class PurchaseOrderItem
{
    public long Id { get; set; }
    public long ItemId { get; set; }
    public required Item Item { get; set; }
    public long PurchaseOrderId { get; set; }
    public required PurchaseOrder PurchaseOrder { get; set; }
    public long? PalletId { get; set; }
    public Pallet? Pallet { get; set; }
    public required double PurchasedQuantity { get; set; }
    public double CurrentQuantity { get; set; }
    public double Weight { get; set; }
    public required double UnitPrice { get; set; }
    public double MarkupPrice { get; set; }
    public double MarginPrice { get; set; }
    public double FreightPrice { get; set; }
    public double SellPrice { get; set; }
}
