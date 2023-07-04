using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

namespace WMSApi.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly ApplicationContext _context;

    public PurchaseOrderService(ApplicationContext context) 
    {
        _context = context; 
    }

    public async Task<PurchaseOrder?> CreatePurchaseOrder(PurchaseOrderCreateDto dto)
    {
        var purchaseOrder = new PurchaseOrder
        {
            Status = PurchaseOrderStatus.Submitted,
            DateCreated = DateTime.Now,
            DateEstimatedDelivery = dto.DateEstimatedDelivery,
            DateLastModified = DateTime.Now,
            Comments = dto.Comments,
            PurchaseOrderItems = new List<PurchaseOrderItem>()
        };

        // Set ID
        _context.PurchaseOrders.Add(purchaseOrder);

        // Add PurchaseOrderItems from DTO
        foreach(var poItemDto in dto.PurchaseOrderItems)
        {
            var item = await _context.Items.FindAsync(poItemDto.ItemId);
            if (item == null)
            {
                return null;
            }

            var poItem = new PurchaseOrderItem
            {
                ItemId = poItemDto.ItemId,
                Item = item,
                PurchaseOrder = purchaseOrder,
                PurchasedQuantity = poItemDto.PurchasedQuantity,
                CurrentQuantity = poItemDto.PurchasedQuantity,
                Weight = poItemDto.Weight,
                UnitPrice = poItemDto.UnitPrice,
                MarkupPrice = poItemDto.MarkupPrice,
                MarginPrice = poItemDto.MarginPrice,
                FreightPrice = poItemDto.FreightPrice,
                SellPrice = poItemDto.UnitPrice + poItemDto.MarkupPrice + poItemDto.MarginPrice + poItemDto.FreightPrice
            };

            purchaseOrder.PurchaseOrderItems.Add(poItem);
        }

        await _context.SaveChangesAsync();
        return purchaseOrder;
    }

    public async Task<PurchaseOrder?> DeletePurchaseOrder(long id)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
        if (purchaseOrder == null)
        {
            return null;
        }

        _context.PurchaseOrders.Remove(purchaseOrder);
        await _context.SaveChangesAsync(); 
        return purchaseOrder;
    }

    public async Task<PurchaseOrder?> GetPurchaseOrderById(long id)
    {
        return await _context.PurchaseOrders.FindAsync(id);
    }

    public async Task<IEnumerable<PurchaseOrder>?> GetPurchaseOrders()
    {
        return await _context.PurchaseOrders.ToListAsync();
    }

    public bool PurchaseOrderExists(long id)
    {
        return (_context.PurchaseOrders?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public async Task<PurchaseOrder?> UpdatePurchaseOrder(long id, PurchaseOrderUpdateDto dto)
    {
        var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
        if (purchaseOrder == null)
        {
            return null;
        }

        // Update

        await _context.SaveChangesAsync();

        return purchaseOrder;
    }
}
