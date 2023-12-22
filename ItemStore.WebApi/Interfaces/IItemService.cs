using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;

namespace ItemStore.WebApi.Interfaces
{
    public interface IItemService
    {
        public Task <Item> Get(int id);
        public Task<IEnumerable<Item>> Get();
        public Task Create(ItemDTO itemDTO);
        public Task EditItem(ItemDTO itemDTO, int id);
        public Task Delete(int id);
        //public decimal Buy(int id, int quantity);
    }
}
