using Xunit;
using Moq;
using AutoFixture.Xunit2;
using ItemStore.WebApi.Clients;
using ItemStore.WebApi.Services;
using ItemStore.WebApi.Model.DTO;
using System.Threading.Tasks;
using System.Collections.Generic;
using ItemStore.WebApi.Exceptions;
using FluentAssertions;

namespace ItemStore.WebApi.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<IJsonPlaceholderClient> _mockClient;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockClient = new Mock<IJsonPlaceholderClient>();
            _userService = new UserService(_mockClient.Object);
        }

        [Theory, AutoData]
        public async Task GetUsersAsync_HappyPath(List<UserDTO> mockUsers)
        {
            _mockClient.Setup(x => x.GetUsersAsync())
                       .ReturnsAsync(new JsonPlaceholderResult<List<UserDTO>> { Data = mockUsers, IsSuccessful = true });

            var result = await _userService.GetUsersAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(mockUsers.Count);
            _mockClient.Verify(x => x.GetUsersAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUsersAsync_ThrowsException()
        {
            _mockClient.Setup(x => x.GetUsersAsync())
                       .ReturnsAsync(new JsonPlaceholderResult<List<UserDTO>> { IsSuccessful = false });

            await Assert.ThrowsAsync<Exception>(() => _userService.GetUsersAsync());
            _mockClient.Verify(x => x.GetUsersAsync(), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetUserAsync_HappyPath(UserDTO mockUser, int userId)
        {
            _mockClient.Setup(x => x.GetUserAsync(userId))
                       .ReturnsAsync(new JsonPlaceholderResult<UserDTO> { Data = mockUser, IsSuccessful = true });

            var result = await _userService.GetUserAsync(userId);

            result.Should().BeEquivalentTo(mockUser);
            _mockClient.Verify(x => x.GetUserAsync(userId), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetUserAsync_ThrowsUserNotFoundException(int userId)
        {
            _mockClient.Setup(x => x.GetUserAsync(userId))
                       .ReturnsAsync(new JsonPlaceholderResult<UserDTO> { IsSuccessful = false });

            await Assert.ThrowsAsync<UserNotFoundException>(() => _userService.GetUserAsync(userId));
            _mockClient.Verify(x => x.GetUserAsync(userId), Times.Once);
        }

        [Theory, AutoData]
        public async Task AddUserAsync_HappyPath(createUserDTO createUserDTO, UserDTO mockUser)
        {
            _mockClient.Setup(x => x.AddUserAsync(createUserDTO))
                       .ReturnsAsync(new JsonPlaceholderResult<UserDTO> { Data = mockUser, IsSuccessful = true });

            var result = await _userService.AddUserAsync(createUserDTO);

            result.Should().BeEquivalentTo(mockUser);
            _mockClient.Verify(x => x.AddUserAsync(createUserDTO), Times.Once);
        }

        [Theory, AutoData]
        public async Task AddUserAsync_ThrowsException(createUserDTO createUserDTO)
        {
            _mockClient.Setup(x => x.AddUserAsync(createUserDTO))
                       .ReturnsAsync(new JsonPlaceholderResult<UserDTO> { IsSuccessful = false });

            await Assert.ThrowsAsync<Exception>(() => _userService.AddUserAsync(createUserDTO));
            _mockClient.Verify(x => x.AddUserAsync(createUserDTO), Times.Once);
        }
    }
}
