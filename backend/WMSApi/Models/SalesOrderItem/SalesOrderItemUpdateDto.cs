namespace WMSApi.Models;

public class SalesOrderItemUpdateDto
{
    public long Id { get; set; }
    public required double Quantity { get; set; }
    public required double UnitPrice { get; set; }
}
