using AutoMapper;
using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;
using ItemStore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ItemStore.WebApi.Controllers
{
    [ApiController]
    [Route("Item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
        }
        [HttpGet("{id:int}")]
        public async Task <ActionResult> Get(int id)
        {
            var item = await _itemService.Get(id);

            return Ok(item);
        }
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            IEnumerable<Item> itemsList =  await _itemService.Get();
            return Ok(itemsList);
        }
        [HttpPost]
        public async Task <ActionResult> CreateItem(ItemDTO item)
        {
            await _itemService.Create(item);
            return NoContent();
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> EditItem(ItemDTO item, int id)
        {
            await _itemService.EditItem(item, id);
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task <ActionResult> Delete(int id)
        {
            await _itemService.Delete(id);
            return NoContent();
        }
        //[HttpGet("{id}/buy")]
        //public IActionResult BuyItem(int id, [FromQuery] int quantity)
        //{
        //    var item = _itemService.Buy(id, quantity);
        //    return Ok(item);
        //}
    }

}

