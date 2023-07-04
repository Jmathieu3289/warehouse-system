using WMSApi.Models;

namespace WMSApi.Services;

public interface IItemService
{
    Task<IEnumerable<Item>?> GetItems();
    Task<Item?> GetItemById(long id);
    Task<Item?> CreateItem(ItemCreateDto dto);
    Task<Item?> UpdateItem(long id, ItemUpdateDto dto);
    Task<Item?> DeleteItem(long id);
    bool ItemExists(long id);
}
