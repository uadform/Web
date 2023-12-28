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

        [Theory]
        [AutoData]
        public async Task Get_GivenValidId_ReturnsDto(int id)
        {
            // Arrange
            _itemRepositoryMock.Setup(m => m.Get(id)).ReturnsAsync(new Item()
            {
                Id = id,
            });
            // Act Assert
            var result = await _itemService.Get(id);
            result.Id.Should().Be(id);
        }

        [Theory]
        [AutoData]
        public async Task Get_GivenValidId_ReturnsItemNotFoundException(int id)
        {
            // Arrange
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
            _itemRepositoryMock.Setup(m => m.Get()).ReturnsAsync(new List<Item>());

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
                          .Returns(Task.CompletedTask);
            // Act
            await _itemService.Create(itemDTO);

            // Assert
            _itemRepositoryMock.Verify(m => m.Create(It.Is<Item>(item => item.Name == itemDTO.Name 
            && item.Price == itemDTO.Price)), Times.Once());
        }

        [Theory]
        [AutoData]
        public async Task EditItem_WithValidItem_EditsRepositoryItem(Item existingItem)
        {
            // Arrange
            var validItemDTO = new ItemDTO { Name = existingItem.Name, Price = existingItem.Price };

            _itemRepositoryMock.Setup(m => m.Get(existingItem.Id)).ReturnsAsync(existingItem);
            _itemRepositoryMock.Setup(m => m.EditItem(It.IsAny<Item>()))
                          .Returns(Task.CompletedTask);

            // Act
            await _itemService.EditItem(validItemDTO, existingItem.Id);

            // Assert
            _itemRepositoryMock.Verify(m => m.EditItem(It.Is<Item>(item =>
                item.Id == existingItem.Id &&
                item.Name == validItemDTO.Name &&
                item.Price == validItemDTO.Price)), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task EditItem_WithInvalidId_ThrowsItemNotFoundException(int invalidId)
        {
            // Arrange
            var itemDTO = new ItemDTO(); // Provide valid itemDTO for your test
            _itemRepositoryMock.Setup(m => m.Get(invalidId)).Returns(Task.FromResult<Item>(null!));

            // Act
            Func<Task> act = async () => await _itemService.EditItem(itemDTO, invalidId);

            // Assert
            await act.Should().ThrowAsync<ItemNotFoundException>();
        }

        [Theory]
        [AutoData]
        public async Task Delete_WithValidId_DeletesRepositoryItem(Item existingItem)
        {
            // Arrange

            _itemRepositoryMock.Setup(m => m.Get(existingItem.Id)).ReturnsAsync(existingItem);
            _itemRepositoryMock.Setup(m => m.Delete(It.IsAny<Item>()))
                          .Returns(Task.CompletedTask)
                          .Verifiable();
            // Act
            await _itemService.Delete(existingItem.Id);

            // Assert
            _itemRepositoryMock.Verify(m => m.Delete(It.Is<Item>(item => item.Id == existingItem.Id)), Times.Once);
        }

        [Theory]
        [AutoData]
        public async Task Delete_WithInvalidId_ThrowsItemNotFoundException(int invalidId)
        {
            // Arrange
            _itemRepositoryMock.Setup(m => m.Get(invalidId)).ReturnsAsync((Item)null!);
            // Act
            Func<Task> act = async () => await _itemService.Delete(invalidId);

            // Assert
            await act.Should().ThrowAsync<ItemNotFoundException>();
            _itemRepositoryMock.Verify(m => m.Get(invalidId), Times.Once);
            _itemRepositoryMock.Verify(m => m.Delete(It.IsAny<Item>()), Times.Never);
        }
    }
}
