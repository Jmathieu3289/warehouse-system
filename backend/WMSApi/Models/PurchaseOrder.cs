namespace WMSApi.Models;

public enum PurchaseOrderStatus
{
    Submitted = 0,
    Received = 1,
    Closed = 2,
    Cancelled = -1
}

public class PurchaseOrder
{
    public long Id { get; set; }
    public PurchaseOrderStatus Status { get; set; }
    public required ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
    public required DateTime DateCreated { get; set; }
    public DateTime? DateEstimatedDelivery { get; set; }
    public DateTime? DateReceived { get; set; }
    public DateTime? DateLastModified { get; set; }
    public string? Comments { get; set;}
}
