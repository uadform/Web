using ItemStore.WebApi.Exceptions;
using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;
using ItemStore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ItemStore.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _purchaseService;

        public PurchaseController(PurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(int id)
        {
            var purchase = await _purchaseService.GetPurchaseByIdAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            return Ok(purchase);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
            var purchases = await _purchaseService.GetAllPurchasesAsync();
            return Ok(purchases);
        }

        [HttpPost]
        public async Task<ActionResult<Purchase>> CreatePurchase([FromBody] CreatePurchaseDTO purchaseDto)
        {
            try
            {
                var purchase = await _purchaseService.CreatePurchaseAsync(purchaseDto);
                return CreatedAtAction(nameof(GetPurchase), new { id = purchase.Id }, purchase);
            }
            catch (UserNotFoundException)
            {
                return NotFound("User not found");
            }
            catch (ItemNotFoundException)
            {
                return NotFound("Item not found");
            }
        }
    }
}
