using WMSApi.Models;

namespace WMSApi.Services;

public interface IPurchaseOrderService
{
    Task<IEnumerable<PurchaseOrder>?> GetPurchaseOrders();
    Task<PurchaseOrder?> GetPurchaseOrderById(long id);
    Task<PurchaseOrder?> CreatePurchaseOrder(PurchaseOrderCreateDto dto);
    Task<PurchaseOrder?> UpdatePurchaseOrder(long id, PurchaseOrderUpdateDto dto);
    Task<PurchaseOrder?> DeletePurchaseOrder(long id);
    bool PurchaseOrderExists(long id);
}
