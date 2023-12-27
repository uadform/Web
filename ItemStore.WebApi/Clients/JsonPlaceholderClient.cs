using ItemStore.WebApi.Model.DTO;
using System.Text;
using System.Text.Json;

namespace ItemStore.WebApi.Clients
{
    public class JsonPlaceholderClient
    {
        private HttpClient _httpClient;

        public JsonPlaceholderClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
            var users = await response.Content.ReadAsAsync<List<UserDTO>>();

            return users;
        }
        public async Task<UserDTO> GetUserById(int id)
        {
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}");
            var user = await response.Content.ReadAsAsync<UserDTO>();
            return user;
        }
        public async Task<UserDTO> AddUser(createUserDTO user)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/users", jsonContent);
            response.EnsureSuccessStatusCode();

            var createdUser = await response.Content.ReadAsAsync<UserDTO>();
            return createdUser;
        }
    }
}
