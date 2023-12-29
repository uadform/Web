using AutoMapper;
using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;
using ItemStore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ItemStore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetAllItems()
        {
            var items = await _itemService.GetAllItemsAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItem(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ItemDTO>> CreateItem([FromBody] ItemDTO itemDto)
        {
            var newItem = await _itemService.CreateItemAsync(itemDto);
            return CreatedAtAction(nameof(GetItem), new { id = newItem.ShopId }, newItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ItemDTO itemDto)
        {
            await _itemService.UpdateItemAsync(id, itemDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _itemService.DeleteItemAsync(id);
            return NoContent();
        }
    }
}

