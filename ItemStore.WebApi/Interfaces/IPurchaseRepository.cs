using ItemStore.WebApi.Model.Entities;

namespace ItemStore.WebApi.Interfaces
{
    public interface IPurchaseRepository
    {
        Task<Purchase> GetPurchaseByIdAsync(int id);
        Task<List<Purchase>> GetAllPurchasesAsync();
        Task<Purchase> CreatePurchaseAsync(Purchase purchase);
    }
}
