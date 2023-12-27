using ItemStore.WebApi.Clients;
using ItemStore.WebApi.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ItemStore.WebApi.Controllers
{
    [ApiController()]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly JsonPlaceholderClient _client;

        public UserController(JsonPlaceholderClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _client.GetUsers());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
                var user = await _client.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(createUserDTO newUser)
        {
            var user = await _client.AddUser(newUser);
            if (user == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
    }
}
