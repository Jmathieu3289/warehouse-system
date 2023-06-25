namespace WMSApi.Models;

public class SalesOrderCreateDto
{
    public required ICollection<SalesOrderItemCreateDto> SalesOrderItems { get; set; }
    public string? Comments { get; set; }
}
