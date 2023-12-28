using ItemStore.WebApi.Model.Entities;

namespace ItemStore.WebApi.Interfaces
{
    public interface IItemRepository
    {
        public Item Get(int id);
        public List<Item> Get();
        public void Create(Item item);
        public void EditItem(Item item);
        public void Delete(Item item);
        public decimal Buy(int id);
    }
}
