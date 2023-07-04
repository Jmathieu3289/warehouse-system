using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

namespace WMSApi.Services;

public class SalesOrderItemService : ISalesOrderItemService
{
    private readonly ApplicationContext _context;

    public SalesOrderItemService(ApplicationContext context) 
    {
        _context = context; 
    }
    
    public async Task<SalesOrderItem?> DeleteSalesOrderItem(long id)
    {
        var salesOrderItem = await _context.SalesOrderItems.FindAsync(id);
        if (salesOrderItem == null)
        {
            return null;
        }

        _context.SalesOrderItems.Remove(salesOrderItem);
        await _context.SaveChangesAsync(); 
        return salesOrderItem;
    }

    public async Task<SalesOrderItem?> GetSalesOrderItemById(long id)
    {
        return await _context.SalesOrderItems.FindAsync(id);
    }

    public async Task<IEnumerable<SalesOrderItem>?> GetSalesOrderItems()
    {
        return await _context.SalesOrderItems.ToListAsync();
    }

    public bool SalesOrderItemExists(long id)
    {
        return (_context.SalesOrderItems?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public async Task<SalesOrderItem?> UpdateSalesOrderItem(long id, SalesOrderItemUpdateDto dto)
    {
        var salesOrderItem = await _context.SalesOrderItems.FindAsync(id);
        if (salesOrderItem == null)
        {
            return null;
        }

        // Update

        await _context.SaveChangesAsync();

        return salesOrderItem;
    }
}
