using ItemStore.WebApi.Model.DTO;
using System.Text;
using System.Text.Json;

namespace ItemStore.WebApi.Clients
{
    public class JsonPlaceholderClient : IJsonPlaceholderClient
    {
        private HttpClient _httpClient;

        public JsonPlaceholderClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<JsonPlaceholderResult<T>> HandleResponse<T>(HttpResponseMessage response, Func<Task<T>> processContent) where T : class
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await processContent();
                return new JsonPlaceholderResult<T>
                {
                    Data = content,
                    IsSuccessful = true,
                    ErrorMessage = ""
                };
            }
            else
            {
                return new JsonPlaceholderResult<T>
                {
                    IsSuccessful = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
        }

        public async Task<JsonPlaceholderResult<List<UserDTO>>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
            return await HandleResponse(response, response.Content.ReadAsAsync<List<UserDTO>>);
        }

        public async Task<JsonPlaceholderResult<UserDTO>> GetUserAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}");
            return await HandleResponse(response, response.Content.ReadAsAsync<UserDTO>);
        }

        public async Task<JsonPlaceholderResult<UserDTO>> AddUserAsync(createUserDTO user)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/users", jsonContent);
            return await HandleResponse(response, response.Content.ReadAsAsync<UserDTO>);
        }
    }   
}
