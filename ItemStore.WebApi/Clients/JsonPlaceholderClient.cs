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

        public async Task<JsonPlaceholderResult<List<UserDTO>>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadAsAsync<List<UserDTO>>();
                return new JsonPlaceholderResult<List<UserDTO>>
                {
                    Data = users,
                    IsSuccessful = true,
                    ErrorMessage = ""
                };
            }
            else
            {
                return new JsonPlaceholderResult<List<UserDTO>>
                {
                    IsSuccessful = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
        }

        public async Task<JsonPlaceholderResult<UserDTO>> GetUserAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}");
            if(response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadAsAsync<UserDTO>();
                return new JsonPlaceholderResult<UserDTO>
                {
                    Data = user,
                    IsSuccessful = true,
                    ErrorMessage = ""
                };
            }
            else
            {
                return new JsonPlaceholderResult<UserDTO>
                {
                    IsSuccessful = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
        }

        public async Task<JsonPlaceholderResult<UserDTO>> AddUserAsync(createUserDTO user)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/users", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var createdUser = await response.Content.ReadAsAsync<UserDTO>();
                return new JsonPlaceholderResult<UserDTO>
                {
                    Data = createdUser,
                    IsSuccessful = true,
                    ErrorMessage = ""
                };
            }
            else
            {
                return new JsonPlaceholderResult<UserDTO>
                {
                    IsSuccessful = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
        }
    }   
}
