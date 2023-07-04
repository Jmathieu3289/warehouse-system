using WMSApi.Models;

namespace WMSApi.Services;

public interface IPurchaseOrderItemService
{
    Task<IEnumerable<PurchaseOrderItem>?> GetPurchaseOrderItems();
    Task<PurchaseOrderItem?> GetPurchaseOrderItemById(long id);
    Task<PurchaseOrderItem?> UpdatePurchaseOrderItem(long id, PurchaseOrderItemUpdateDto dto);
    Task<PurchaseOrderItem?> DeletePurchaseOrderItem(long id);
    bool PurchaseOrderItemExists(long id);
}
