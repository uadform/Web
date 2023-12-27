using Moq;
using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Services;
using AutoMapper;
using ItemStore.WebApi.Mappings;
using FluentAssertions;
using ItemStore.WebApi.Exceptions;
using ItemStore.WebApi.Model.Entities;
using ItemStore.WebApi.Model.DTO;
using System.Runtime.CompilerServices;
using System.Configuration;
using FluentAssertions.Common;
using AutoFixture;
using AutoFixture.Xunit2;

namespace ItemStore.UnitTests.Services
{
    public class ItemServiceTests
    {
        private readonly ItemService _itemService;
        private readonly Mock<IEFCoreRepository> _itemRepositoryMock;
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public ItemServiceTests()
        {
            _itemRepositoryMock = new Mock<IEFCoreRepository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            _mapper = configuration.CreateMapper();

            _itemService = new ItemService(_itemRepositoryMock.Object, _mapper);
            _fixture = new Fixture();
        }

        [Fact]
        public async Task Get_GivenValidId_ReturnsDto()
        {
            // Arrange
            int id = _fixture.Create<int>();
            _itemRepositoryMock.Setup(m => m.Get(id)).ReturnsAsync(new Item()
            {
                Id = id,
            });
            // Act Assert
            var result = await _itemService.Get(id);
            result.Id.Should().Be(id);
        }
        [Fact]
        public async Task Get_GivenValidId_ReturnsItemNotFoundException()
        {
            // Arrange
            int id = _fixture.Create<int>();
            _itemRepositoryMock.Setup(m => m.Get(id)).Returns(Task.FromResult<Item>(null));

            // Act
            Func<Task> act = async () => await _itemService.Get(id);

            // Assert
            await act.Should().ThrowAsync<ItemNotFoundException>();
        }

        [Theory]
        [AutoData]
        public async Task GetAllItems_WhenCalled_ReturnsAllItems(List<Item> testItems)
        {
            // Arrange
            _itemRepositoryMock.Setup(m => m.Get()).ReturnsAsync(testItems);

            // Act
            var result = await _itemService.Get();

            // Assert
            result.Should().NotBeNull();
            testItems.Count.Should().Be(result.Count());
        }
        [Fact]
        public async Task GetAllItems_WhenNoItems_ReturnsEmptyException()
        {
            // Arrange
            var testItems = new List<Item>(); // Empty list

            _itemRepositoryMock.Setup(m => m.Get()).ReturnsAsync(testItems);

            // Act
            Func<Task> act = async () => await _itemService.Get();

            // Assert
            await act.Should().ThrowAsync<ItemListEmptyException>();
        }
        [Theory]
        [AutoData]
        public async Task CreateAnItem_WhenCalled_MakesRepositoryCreateWithMappedItem(ItemDTO itemDTO)
        {
            _itemRepositoryMock.Setup(m => m.Create(It.IsAny<Item>()))
                          .Returns(Task.CompletedTask)
                          .Verifiable(); // Important to make sure this method is called
            // Act
            await _itemService.Create(itemDTO);

            // Assert
            _itemRepositoryMock.Verify(m => m.Create(It.Is<Item>(item => item.Name == itemDTO.Name && item.Price == itemDTO.Price)), Times.AtLeastOnce());
        }
        [Theory]
        [InlineData("Name1", 5, 1)] // Example data set 1
        [InlineData("Name2", 10, 2)] // Example data set 2
        public async Task EditItem_WithValidItem_EditsRepositoryItem(string name, decimal price, int validId)
        {
            // Arrange
            var validItemDTO = new ItemDTO { Name = name, Price = price };
            var existingItem = new Item { Id = validId, Name = name, Price = price };

            _itemRepositoryMock.Setup(m => m.Get(validId)).ReturnsAsync(existingItem);
            _itemRepositoryMock.Setup(m => m.EditItem(It.IsAny<Item>()))
                          .Returns(Task.CompletedTask)
                          .Verifiable();

            // Act
            await _itemService.EditItem(validItemDTO, validId);

            // Assert
            _itemRepositoryMock.Verify(m => m.EditItem(It.Is<Item>(item =>
                item.Id == validId &&
                item.Name == validItemDTO.Name &&
                item.Price == validItemDTO.Price)), Times.Once);
        }

        [Theory]
        [InlineAutoData(-1)]
        [InlineAutoData(0)]
        public async Task EditItem_WithInvalidId_ThrowsItemNotFoundException(int invalidId, ItemDTO itemDTO)
        {
            // Arrange
            _itemRepositoryMock.Setup(m => m.Get(It.IsAny<int>())).ReturnsAsync((int id) => id < 1 ? null : new Item());

            // Act
            Func<Task> act = async () => await _itemService.EditItem(itemDTO, invalidId);

            // Assert
            await act.Should().ThrowAsync<ItemNotFoundException>();
        }
        [Theory]
        [AutoData]
        public async Task Delete_WithValidId_DeletesRepositoryItem(int validId, string name, decimal price)
        {
            // Arrange
            var existingItem = new Item { Id = validId, Name = name, Price = price};

            _itemRepositoryMock.Setup(m => m.Get(validId)).ReturnsAsync(existingItem);
            _itemRepositoryMock.Setup(m => m.Delete(It.IsAny<Item>()))
                          .Returns(Task.CompletedTask)
                          .Verifiable();
            // Act
            await _itemService.Delete(validId);

            // Assert
            _itemRepositoryMock.Verify(m => m.Delete(It.Is<Item>(item => item.Id == validId)), Times.Once);
        }
        [Fact]
        public async Task Delete_WithInvalidId_ThrowsItemNotFoundException()
        {
            // Arrange
            int invalidId = -1;

            _itemRepositoryMock.Setup(m => m.Get(invalidId)).ReturnsAsync((Item)null);
            // Act
            Func<Task> act = async () => await _itemService.Delete(invalidId);

            // Assert
            await act.Should().ThrowAsync<ItemNotFoundException>();
        }
    }
}
