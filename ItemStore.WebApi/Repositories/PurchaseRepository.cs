using ItemStore.WebApi.Contexts;
using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItemStore.WebApi.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly DataContext _context;

        public PurchaseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Purchase> GetPurchaseByIdAsync(int id)
        {
            return await _context.Purchases
                                 .Include(p => p.Item)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Purchase>> GetAllPurchasesAsync()
        {
            return await _context.Purchases
                                 .Include(p => p.Item)
                                 .ToListAsync();
        }

        public async Task<Purchase> CreatePurchaseAsync(Purchase purchase)
        {
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return purchase;
        }
    }
}
