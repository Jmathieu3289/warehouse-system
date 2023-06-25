namespace WMSApi.Models;

public class PurchaseOrderCreateDto
{
    public required ICollection<PurchaseOrderItemCreateDto> PurchaseOrderItems { get; set; }
    public DateTime? DateEstimatedDelivery { get; set; }
    public string? Comments { get; set; }
}
