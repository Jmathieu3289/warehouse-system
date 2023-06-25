namespace WMSApi.Models;

public class PurchaseOrderUpdateDto
{
    public long Id { get; set; }
    public PurchaseOrderStatus Status { get; set; }
    public DateTime? DateEstimatedDelivery { get; set; }
    public DateTime? DateReceived { get; set; }
    public string? Comments { get; set; }
}
