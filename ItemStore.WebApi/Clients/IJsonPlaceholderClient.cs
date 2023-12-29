using ItemStore.WebApi.Model.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItemStore.WebApi.Clients
{
    public interface IJsonPlaceholderClient
    {
        Task<JsonPlaceholderResult<List<UserDTO>>> GetUsersAsync();
        Task<JsonPlaceholderResult<UserDTO>> GetUserAsync(int id);
        Task<JsonPlaceholderResult<UserDTO>> AddUserAsync(createUserDTO user);
    }
}
