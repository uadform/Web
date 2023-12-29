using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;

namespace ItemStore.WebApi.Interfaces
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<ItemDTO> GetItemByIdAsync(int id);
        Task<ItemDTO> CreateItemAsync(ItemDTO itemDto);
        Task UpdateItemAsync(int id, ItemDTO itemDto);
        Task DeleteItemAsync(int id);
    }
}
