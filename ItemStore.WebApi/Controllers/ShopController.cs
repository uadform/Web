using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ItemStore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopDTO>>> GetAllShops()
        {
            var shopDtos = await _shopService.GetAllShopsAsync();
            return Ok(shopDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shop>> GetShop(int id)
        {
            var shop = await _shopService.GetShopByIdAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            return shop;
        }

        [HttpPost]
        public async Task<ActionResult> CreateShop([FromBody] ShopDTO newShop)
        {
            var shop = await _shopService.CreateShopAsync(newShop);
            return CreatedAtAction(nameof(GetShop), new { id = shop.Id }, shop);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateShop(int id, [FromBody] ShopDTO shop)
        {
            await _shopService.UpdateShopAsync(id, shop);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var shop = await _shopService.GetShopByIdAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            await _shopService.DeleteShopAsync(id);
            return NoContent();
        }
    }

}
