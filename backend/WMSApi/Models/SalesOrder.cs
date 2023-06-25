namespace WMSApi.Models;

public enum SalesOrderStatus
{
    Submitted = 0,
    Filling = 1,
    Staged = 2,
    InTransit = 3,
    Closed = 4,
    Cancelled = -1,
    Disputed = -2
}

public class SalesOrder
{
    public long Id { get; set; }
    public SalesOrderStatus Status { get; set; }
    public required ICollection<SalesOrderItem> SalesOrderItems { get; set; }
    public required DateTime DateCreated { get; set; }
    public DateTime? DateFilling { get; set; }
    public DateTime? DateStaged { get; set; }
    public DateTime? DateShipped { get; set; }
    public DateTime? DateReceived { get; set; }
    public DateTime? DateLastModified { get; set; }
    public string? Comments { get; set; }
}
