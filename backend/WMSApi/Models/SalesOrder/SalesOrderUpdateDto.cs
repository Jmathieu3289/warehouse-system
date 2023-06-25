namespace WMSApi.Models;

public class SalesOrderUpdateDto
{
    public long Id { get; set; }
    public SalesOrderStatus Status { get; set; }
    public string? Comments { get; set; }
}
