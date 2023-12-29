using ItemStore.WebApi.Contexts;
using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ItemStore.WebApi.Repositories
{
    public class EFCoreRepository : IEFCoreRepository
    {
        private readonly DataContext _dataContext;
        public EFCoreRepository(DataContext dataContext) { _dataContext = dataContext; }

        public async Task<List<Item>> Get()
        {
            return await _dataContext.Items.ToListAsync();
        }
        public async Task<Item> Get(int id)
        {
            return await _dataContext.Items.FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task Create(Item item)
        {
            _dataContext.Items.Add(item);
            await _dataContext.SaveChangesAsync();
        }
        public async Task Update(Item item)
        {
            _dataContext.Items.Update(item);
            await _dataContext.SaveChangesAsync();
        }
        public async Task EditItem(Item item)
        {
            _dataContext.Items.Update(item);
            await _dataContext.SaveChangesAsync();
        }
        public async Task Delete(Item item)
        {
            _dataContext.Items.Remove(item);
            await _dataContext.SaveChangesAsync();
        }
    }
}
