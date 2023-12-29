using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ItemStore.WebApi.Interfaces
{
    public interface IShopService
    {
        public Task<IEnumerable<ShopDTO>> GetAllShopsAsync();
        Task<Shop> GetShopByIdAsync(int shopId);
        public Task<Shop> CreateShopAsync(ShopDTO newShopDto);
        Task UpdateShopAsync(int id, [FromBody] ShopDTO shop);
        Task DeleteShopAsync(int shopId);
    }

}
