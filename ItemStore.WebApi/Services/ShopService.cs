using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemStore.WebApi.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _shopRepository;

        public ShopService(IShopRepository shopRepository)
        {
            _shopRepository = shopRepository;
        }

        public async Task<IEnumerable<ShopDTO>> GetAllShopsAsync()
        {
            var shops = await _shopRepository.GetAllShopsAsync();
            return shops.Select(shop => new ShopDTO
            {
                Id = shop.Id,
                Name = shop.Name,
                Address = shop.Address,
                Items = shop.Items.Select(item => new ItemDTO
                {
                    Name = item.Name,
                    Price = item.Price,
                    ShopId = item.ShopId
                }).ToList()
            });
        }
        public Task<Shop> GetShopByIdAsync(int shopId)
        {
            return _shopRepository.GetShopByIdAsync(shopId);
        }

        public async Task<Shop> CreateShopAsync(ShopDTO newShopDto)
        {
            var shop = new Shop
            {
                Name = newShopDto.Name,
                Address = newShopDto.Address
            };

            return await _shopRepository.CreateShopAsync(shop);
        }

        public async Task UpdateShopAsync(int id, ShopDTO shopDto)
        {
            var shop = await _shopRepository.GetShopByIdAsync(id);
            if (shop == null)
            {
                throw new Exception("Shop not found"); // Custom exception
            }

            shop.Name = shopDto.Name;
            shop.Address = shopDto.Address;
            // Update other properties as needed

            await _shopRepository.UpdateShopAsync(shop);
        }


        public Task DeleteShopAsync(int shopId)
        {
            return _shopRepository.DeleteShopAsync(shopId);
        }
    }

}
