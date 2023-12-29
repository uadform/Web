using ItemStore.WebApi.Contexts;
using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemStore.WebApi.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly DataContext _context;

        public ShopRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shop>> GetAllShopsAsync()
        {
            return await _context.Shops
                         .Include(shop => shop.Items)
                         .ToListAsync();
        }

        public async Task<Shop> GetShopByIdAsync(int shopId)
        {
            return await _context.Shops
                         .Include(shop => shop.Items)
                         .FirstOrDefaultAsync(shop => shop.Id == shopId);
        }

        public async Task<Shop> CreateShopAsync(Shop shop)
        {
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
            return shop;
        }

        public async Task UpdateShopAsync(Shop shop)
        {   
            _context.Entry(shop).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShopAsync(int shopId)
        {
            var shop = await _context.Shops.FindAsync(shopId);
            if (shop != null)
            {
                _context.Shops.Remove(shop);
                await _context.SaveChangesAsync();
            }
        }
    }

}
