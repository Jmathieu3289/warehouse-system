using WMSApi.Models;

namespace WMSApi.Services;

public interface ISalesOrderItemService
{
    Task<IEnumerable<SalesOrderItem>?> GetSalesOrderItems();
    Task<SalesOrderItem?> GetSalesOrderItemById(long id);
    Task<SalesOrderItem?> UpdateSalesOrderItem(long id, SalesOrderItemUpdateDto dto);
    Task<SalesOrderItem?> DeleteSalesOrderItem(long id);
    bool SalesOrderItemExists(long id);
}
