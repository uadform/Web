using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;

namespace ItemStore.WebApi.Interfaces
{
    public interface IShopRepository
    {
        Task<IEnumerable<Shop>> GetAllShopsAsync();
        Task<Shop> GetShopByIdAsync(int shopId);
        Task <Shop> CreateShopAsync(Shop shop);
        public Task UpdateShopAsync(Shop shop);
        Task DeleteShopAsync(int shopId);
    }

}
