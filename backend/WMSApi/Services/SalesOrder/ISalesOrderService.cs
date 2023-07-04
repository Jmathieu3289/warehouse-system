using WMSApi.Models;

namespace WMSApi.Services;

public interface ISalesOrderService
{
    Task<IEnumerable<SalesOrder>?> GetSalesOrders();
    Task<SalesOrder?> GetSalesOrderById(long id);
    Task<SalesOrder?> CreateSalesOrder(SalesOrderCreateDto dto);
    Task<SalesOrder?> UpdateSalesOrder(long id, SalesOrderUpdateDto dto);
    Task<SalesOrder?> DeleteSalesOrder(long id);
    bool SalesOrderExists(long id);
}
