using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

namespace WMSApi.Services;

public class PurchaseOrderItemService : IPurchaseOrderItemService
{
    private readonly ApplicationContext _context;

    public PurchaseOrderItemService(ApplicationContext context) 
    {
        _context = context; 
    }

    public async Task<PurchaseOrderItem?> DeletePurchaseOrderItem(long id)
    {
        var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
        if (purchaseOrderItem == null)
        {
            return null;
        }

        _context.PurchaseOrderItems.Remove(purchaseOrderItem);
        await _context.SaveChangesAsync(); 
        return purchaseOrderItem;
    }

    public async Task<PurchaseOrderItem?> GetPurchaseOrderItemById(long id)
    {
        return await _context.PurchaseOrderItems.FindAsync(id);
    }

    public async Task<IEnumerable<PurchaseOrderItem>?> GetPurchaseOrderItems()
    {
        return await _context.PurchaseOrderItems.ToListAsync();
    }

    public bool PurchaseOrderItemExists(long id)
    {
        return (_context.PurchaseOrderItems?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public async Task<PurchaseOrderItem?> UpdatePurchaseOrderItem(long id, PurchaseOrderItemUpdateDto dto)
    {
        var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
        if (purchaseOrderItem == null)
        {
            return null;
        }

        // Update

        await _context.SaveChangesAsync();

        return purchaseOrderItem;
    }
}
