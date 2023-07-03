using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

namespace WMSApi.Services;

public class ItemService : IItemService
{
    private readonly ApplicationContext _context;

    public ItemService(ApplicationContext context) 
    {
        _context = context; 
    }

    public async Task<IEnumerable<Item>?> GetItems()
    {
        return await _context.Items.ToListAsync();
    }

    public async Task<Item?> GetItemById(long id)
    {
        return await _context.Items.FindAsync(id);
    }

    public async Task<Item?> UpdateItem(long id, ItemUpdateDto itemDto)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null)
        {
            return null;
        }

        item.Name = itemDto.Name;
        item.UPC = itemDto.UPC;
        item.DateLastModified = DateTime.Now;

        await _context.SaveChangesAsync();

        return item;
    }

    public async Task<Item?> CreateItem(ItemCreateDto itemDTO)
    {
        var item = new Item
        {
            Name = itemDTO.Name,
            UPC = itemDTO.UPC,
            DateCreated = DateTime.Now,
            DateLastModified = DateTime.Now
        };

        _context.Items.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<Item?> DeleteItem(long id)
    {
        var item = await _context.Items.FindAsync(id);
        if (item == null)
        {
            return null;
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync(); 
        return item;
    }

    public bool ItemExists(long id)
    {
        return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
