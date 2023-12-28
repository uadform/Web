using ItemStore.WebApi.Clients;
using ItemStore.WebApi.Exceptions;
using ItemStore.WebApi.Model.DTO;

namespace ItemStore.WebApi.Services
{
    public class UserService
    {
        private readonly JsonPlaceholderClient _client;

        public UserService(JsonPlaceholderClient client)
        {
            _client = client;
        }

        public async Task<List<UserDTO>> GetUsersAsync()
        {
            var result = await _client.GetUsersAsync();
            if (!result.IsSuccessful) throw new Exception("Failed to retrieve users");
            return result.Data ?? new List<UserDTO>();
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            var result = await _client.GetUserAsync(id);
            if (!result.IsSuccessful) throw new UserNotFoundException();
            return result.Data ?? throw new UserNotFoundException();
        }

        public async Task<UserDTO> AddUserAsync(createUserDTO user)
        {
            var result = await _client.AddUserAsync(user);
            if (!result.IsSuccessful) throw new Exception("Failed to add user");
            return result.Data ?? throw new Exception("User creation failed");
        }
    }
}
