namespace WMSApi.Models;

public class SalesOrderItem
{
    public long Id { get; set; }
    public long SalesOrderId { get; set; }
    public required SalesOrder SalesOrder { get; set; }
    public long PurchaseOrderItemId { get; set; }
    public required PurchaseOrderItem PurchaseOrderItem { get; set; }
    public required double Quantity { get; set; }
    public required double UnitPrice { get; set; }
}
