using ItemStore.WebApi.Repositories;
using ItemStore.WebApi.Interfaces;
using System.Xml.Linq;
using ItemStore.WebApi.Model.Entities;
using ItemStore.WebApi.Model.DTO;
using AutoMapper;
using ItemStore.WebApi.Exceptions;
using static Dapper.SqlMapper;
namespace ItemStore.WebApi.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;

        public ItemService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            var items = await _itemRepository.GetAllItemsAsync();
            return items.Select(i => new Item
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price,
                ShopId = i.ShopId
            });
        }

        public async Task<ItemDTO> GetItemByIdAsync(int id)
        {
            var item = await _itemRepository.GetItemByIdAsync(id);
            if (item == null) return null;

            return new ItemDTO
            {
                Name = item.Name,
                Price = item.Price,
                ShopId = item.ShopId
            };
        }

        public async Task<ItemDTO> CreateItemAsync(ItemDTO itemDto)
        {
            var newItem = new Item
            {
                Name = itemDto.Name,
                Price = itemDto.Price,
                ShopId = itemDto.ShopId
            };

            var createdItem = await _itemRepository.CreateItemAsync(newItem);

            return new ItemDTO
            {
                Name = createdItem.Name,
                Price = createdItem.Price,
                ShopId = createdItem.ShopId
            };
        }

        public async Task UpdateItemAsync(int id, ItemDTO itemDto)
        {
            var item = await _itemRepository.GetItemByIdAsync(id);
            if (item == null) throw new Exception("Item not found");

            item.Name = itemDto.Name;
            item.Price = itemDto.Price;
            item.ShopId = itemDto.ShopId;

            await _itemRepository.UpdateItemAsync(item);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _itemRepository.DeleteItemAsync(id);
        }
    }

}

