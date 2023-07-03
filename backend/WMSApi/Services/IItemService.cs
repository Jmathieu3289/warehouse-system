using WMSApi.Models;

namespace WMSApi.Services;

public interface IItemService
{
    Task<IEnumerable<Item>?> GetItems();
    Task<Item?> GetItemById(long id);
    Task<Item?> CreateItem(ItemCreateDto itemDTO);
    Task<Item?> UpdateItem(long id, ItemUpdateDto itemDto);
    Task<Item?> DeleteItem(long id);
    bool ItemExists(long id);
}
