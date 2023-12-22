using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItemStore.WebApi.Interfaces;
using ItemStore.WebApi.Services;
using AutoMapper;
using ItemStore.WebApi.Mappings;
using FluentAssertions;
using ItemStore.WebApi.Exceptions;
using ItemStore.WebApi.Model.Entities;
using ItemStore.WebApi.Model.DTO;

namespace ItemStore.UnitTests.Services
{
    public class ItemServiceTests
    {
        [Fact]
        public async Task Get_GivenValidId_ReturnsDto()
        {
            int id = 1;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = configuration.CreateMapper();
            var testRepository = new Mock<IEFCoreRepository>();
            testRepository.Setup(m => m.Get(id)).ReturnsAsync(new WebApi.Model.Entities.Item()
            {
                Id = id,
            });
            var itemService = new ItemService(testRepository.Object, mapper);
            var result = await itemService.Get(id);
            result.Id.Should().Be(id);
        }
        [Fact]
        public async Task Get_GivenValidId_ReturnsItemNotFoundException()
        {
            int id = 1;
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = configuration.CreateMapper();
            var testRepository = new Mock<IEFCoreRepository>();
            testRepository.Setup(m => m.Get(id)).Returns(Task.FromResult<Item>(null));
            var itemService = new ItemService(testRepository.Object, mapper);
            await Assert.ThrowsAsync<ItemNotFoundException>(async () => await itemService.Get(id));
        }
        [Fact]
        public async Task GetAllItems_WhenCalled_ReturnsAllItems()
        {
            // Arrange
            var testItems = new List<Item>
            {
                new Item { Id = 1, /* other properties */ },
                new Item { Id = 2, /* other properties */ }
            };

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = configuration.CreateMapper();

            var testRepository = new Mock<IEFCoreRepository>();
            testRepository.Setup(m => m.Get()).ReturnsAsync(testItems);

            var itemService = new ItemService(testRepository.Object, mapper);

            // Act
            var result = await itemService.Get();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testItems.Count, result.Count());
        }
        [Fact]
        public async Task GetAllItems_WhenNoItems_ReturnsEmptyException()
        {
            // Arrange
            var testItems = new List<Item>(); // Empty list

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = configuration.CreateMapper();

            var testRepository = new Mock<IEFCoreRepository>();
            testRepository.Setup(m => m.Get()).ReturnsAsync(testItems);

            var itemService = new ItemService(testRepository.Object, mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ItemListEmptyException>(() => itemService.Get());
        }
        [Fact]
        public async Task CreateAnItem_WhenCalled_MakesRepositoryCreateWithMappedItem()
        {
            var itemDTO = new ItemDTO() { Name = "Name", Price = 5 }; // Populate with valid data if necessary

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = configuration.CreateMapper();

            var testRepository = new Mock<IEFCoreRepository>();
            testRepository.Setup(m => m.Create(It.IsAny<Item>()))
                          .Returns(Task.CompletedTask)
                          .Verifiable(); // Important to make sure this method is called

            var itemService = new ItemService(testRepository.Object, mapper);

            // Act
            await itemService.Create(itemDTO);

            // Assert
            testRepository.Verify(m => m.Create(It.Is<Item>(item => item.Name == itemDTO.Name && item.Price == itemDTO.Price)), Times.AtLeastOnce());
        }
        [Theory]
        [InlineData("Name1", 5, 1)] // Example data set 1
        [InlineData("Name2", 10, 2)] // Example data set 2
        public async Task EditItem_WithValidItem_EditsRepositoryItem(string name, decimal price, int validId)
        {
            // Arrange
            var validItemDTO = new ItemDTO { Name = name, Price = price };
            var existingItem = new Item { Id = validId, Name = name, Price = price };

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = configuration.CreateMapper();

            var testRepository = new Mock<IEFCoreRepository>();
            testRepository.Setup(m => m.Get(validId)).ReturnsAsync(existingItem);
            testRepository.Setup(m => m.EditItem(It.IsAny<Item>()))
                          .Returns(Task.CompletedTask)
                          .Verifiable();

            var itemService = new ItemService(testRepository.Object, mapper);

            // Act
            await itemService.EditItem(validItemDTO, validId);

            // Assert
            testRepository.Verify(m => m.EditItem(It.Is<Item>(item =>
                item.Id == validId &&
                item.Name == validItemDTO.Name &&
                item.Price == validItemDTO.Price)), Times.Once);
        }
        [Theory]
        [InlineData(-1)] // Example invalid ID 1
        [InlineData(0)]  // Example invalid ID 2
        public async Task EditItem_WithInvalidId_ThrowsItemNotFoundException(int invalidId)
        {
            // Arrange
            var itemDTO = new ItemDTO {Name = "Name", Price = 1};

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = configuration.CreateMapper();

            var testRepository = new Mock<IEFCoreRepository>();
            // Explicitly indicating the nullable return type
            testRepository.Setup(m => m.Get(invalidId)).ReturnsAsync((Item?)null);

            var itemService = new ItemService(testRepository.Object, mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ItemNotFoundException>(() => itemService.EditItem(itemDTO, invalidId));
        }
        [Fact]
        public async Task Delete_WithValidId_DeletesRepositoryItem()
        {
            // Arrange
            int validId = 1;
            string name = "Name";
            decimal price = 5;
            var existingItem = new Item { Id = validId, Name = name, Price = price};

            var testRepository = new Mock<IEFCoreRepository>();
            testRepository.Setup(m => m.Get(validId)).ReturnsAsync(existingItem);
            testRepository.Setup(m => m.Delete(It.IsAny<Item>()))
                          .Returns(Task.CompletedTask)
                          .Verifiable();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = configuration.CreateMapper();

            var itemService = new ItemService(testRepository.Object, mapper);

            // Act
            await itemService.Delete(validId);

            // Assert
            testRepository.Verify(m => m.Delete(It.Is<Item>(item => item.Id == validId)), Times.Once);
        }
        [Fact]
        public async Task Delete_WithInvalidId_ThrowsItemNotFoundException()
        {
            // Arrange
            int invalidId = -1;

            var testRepository = new Mock<IEFCoreRepository>();
            testRepository.Setup(m => m.Get(invalidId)).ReturnsAsync((Item)null);
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            var mapper = configuration.CreateMapper();
            var itemService = new ItemService(testRepository.Object, mapper);

            // Act & Assert
            await Assert.ThrowsAsync<ItemNotFoundException>(() => itemService.Delete(invalidId));
        }








    }
}
