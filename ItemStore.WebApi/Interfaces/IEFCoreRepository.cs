using ItemStore.WebApi.Model.Entities;

namespace ItemStore.WebApi.Interfaces
{
    public interface IEFCoreRepository
    {
        Task Create(Item item);
        Task Delete(Item item);
        Task EditItem(Item item);
        Task<List<Item>> Get();
        Task<Item> Get(int id);
        Task Update(Item item);
    }
}