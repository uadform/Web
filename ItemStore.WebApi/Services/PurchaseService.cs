using ItemStore.WebApi.Exceptions;
using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Model.Entities;

namespace ItemStore.WebApi.Services
{
    public class PurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly UserService _userService;
        private readonly IItemRepository _itemRepository;

        public PurchaseService(IPurchaseRepository purchaseRepository, UserService userService, IItemRepository itemRepository)
        {
            _purchaseRepository = purchaseRepository;
            _userService = userService;
            _itemRepository = itemRepository;
        }
        public async Task<Purchase> GetPurchaseByIdAsync(int id)
        {
            return await _purchaseRepository.GetPurchaseByIdAsync(id);
        }

        public async Task<List<Purchase>> GetAllPurchasesAsync()
        {
            return await _purchaseRepository.GetAllPurchasesAsync();
        }
        public async Task<Purchase> CreatePurchaseAsync(CreatePurchaseDTO purchaseDto)
        {
            var user = await _userService.GetUserAsync(purchaseDto.UserId) ?? throw new UserNotFoundException();
            var item = await _itemRepository.GetItemByIdAsync(purchaseDto.ItemId) ?? throw new ItemNotFoundException();

            var purchase = new Purchase
            {
                UserId = purchaseDto.UserId,
                ItemId = purchaseDto.ItemId,
                PurchaseDate = DateTime.UtcNow
            };

            return await _purchaseRepository.CreatePurchaseAsync(purchase);
        }
    }
}
