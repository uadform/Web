using System.Data;
using Dapper;
using ItemStore.WebApi.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ItemStore.WebApi.Model.Entities;
using ItemStore.WebApi.Contexts;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ItemStore.WebApi.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataContext _context;

        public ItemRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item> GetItemByIdAsync(int id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task<Item> CreateItemAsync(Item item)
        {
            _context.Items.Add(item);

            if (item.ShopId.HasValue)
            {
                var shop = await _context.Shops
                                         .Include(s => s.Items)
                                         .FirstOrDefaultAsync(s => s.Id == item.ShopId.Value);
                if (shop != null)
                {
                    shop.Items.Add(item);
                }
            }

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateItemAsync(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await GetItemByIdAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}
