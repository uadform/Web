using ItemStore.WebApi.Repositories;
using ItemStore.WebApi.Interfaces;
using System.Xml.Linq;
using ItemStore.WebApi.Model.Entities;
using ItemStore.WebApi.Model.DTO;
using AutoMapper;
using ItemStore.WebApi.Exceptions;
using static Dapper.SqlMapper;
namespace ItemStore.WebApi.Services
{
    public class ItemService : IItemService
    {
        private readonly IEFCoreRepository _itemRepository;
        private readonly IMapper _mapper;
        public ItemService(IEFCoreRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }
        public async Task<Item> Get(int id)
        {
            var entity = await _itemRepository.Get(id);
            if (entity == null) throw new ItemNotFoundException();
            return entity;
        }
        public async Task<IEnumerable<Item>> Get()
        {   var entity = await _itemRepository.Get();
            if(entity.Count == 0) throw new ItemListEmptyException();
            return entity;
        }
        public async Task Create(ItemDTO itemDTO)
        {
            var item = _mapper.Map<Item>(itemDTO);
            await _itemRepository.Create(item);
        }
        public async Task EditItem(ItemDTO itemDTO, int id)
        {
            var entity = await _itemRepository.Get(id);
            if (entity == null) throw new ItemNotFoundException();
            var item = _mapper.Map<Item>(itemDTO);
            item.Id = id;
            await _itemRepository.EditItem(item);
        }
        public async Task Delete(int id)
        {
            var entity = await _itemRepository.Get(id);
            if (entity == null) throw new ItemNotFoundException();
            await _itemRepository.Delete(entity);
        }
        //public int DeleteItem(int id)
        //{
        //    return _itemRepository.DeleteItemById(id);
        //}
        //public decimal Buy(int id, int quantity)
        //{
        //    if (GetItemsByID(id) != null)
        //    {
        //        Item item = GetItemsByID(id);

        //        if (quantity >= 20)
        //        {
        //            item.Price = item.Price * 0.8m * quantity;

        //        }
        //        else if (quantity >= 10)
        //        {
        //            item.Price = item.Price * 0.9m * quantity;

        //        }
        //        else return item.Price = item.Price * quantity;
        //        return item.Price;
        //    }
        //    return 0;
        //}
    }
}
