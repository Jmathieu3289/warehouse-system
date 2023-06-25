namespace WMSApi.Models;

public class SalesOrderItemCreateDto
{
    public long SalesOrderId { get; set; }
    public long PurchaseOrderItemId { get; set; }
    public required double Quantity { get; set; }
    public required double UnitPrice { get; set; }
}
