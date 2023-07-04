using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

namespace WMSApi.Services;

public class SalesOrderService : ISalesOrderService
{
    private readonly ApplicationContext _context;

    public SalesOrderService(ApplicationContext context) 
    {
        _context = context; 
    }
    
    public async Task<SalesOrder?> CreateSalesOrder(SalesOrderCreateDto dto)
    {
        var salesOrder = new SalesOrder
        {
            Status = SalesOrderStatus.Submitted,
            DateCreated = DateTime.Now,
            DateLastModified = DateTime.Now,
            Comments = dto.Comments,
            SalesOrderItems = new List<SalesOrderItem>()
        };

        // Set ID
        _context.SalesOrders.Add(salesOrder);

        // Add SalesOrderItems from DTO
        foreach(var soItemDto in dto.SalesOrderItems)
        {
            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(soItemDto.PurchaseOrderItemId);
            if (purchaseOrderItem == null)
            {
                return null;
            }

            var soItem = new SalesOrderItem
            {
                PurchaseOrderItem = purchaseOrderItem,
                PurchaseOrderItemId = soItemDto.PurchaseOrderItemId,
                SalesOrder = salesOrder,
                Quantity = soItemDto.Quantity,
                UnitPrice = soItemDto.UnitPrice
            };

            salesOrder.SalesOrderItems.Add(soItem);
        }

        await _context.SaveChangesAsync();
        return salesOrder;
    }

    public async Task<SalesOrder?> DeleteSalesOrder(long id)
    {
        var salesOrder = await _context.SalesOrders.FindAsync(id);
        if (salesOrder == null)
        {
            return null;
        }

        _context.SalesOrders.Remove(salesOrder);
        await _context.SaveChangesAsync(); 
        return salesOrder;
    }

    public async Task<SalesOrder?> GetSalesOrderById(long id)
    {
        return await _context.SalesOrders.FindAsync(id);
    }

    public async Task<IEnumerable<SalesOrder>?> GetSalesOrders()
    {
        return await _context.SalesOrders.ToListAsync();
    }

    public bool SalesOrderExists(long id)
    {
        return (_context.SalesOrders?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public async Task<SalesOrder?> UpdateSalesOrder(long id, SalesOrderUpdateDto dto)
    {
        var salesOrder = await _context.SalesOrders.FindAsync(id);
        if (salesOrder == null)
        {
            return null;
        }

        // Update

        await _context.SaveChangesAsync();

        return salesOrder;
    }
}
