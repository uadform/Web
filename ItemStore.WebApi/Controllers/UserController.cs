using ItemStore.WebApi.Clients;
using ItemStore.WebApi.Model.DTO;
using ItemStore.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ItemStore.WebApi.Controllers
{
    [ApiController()]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetUsersAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetUserAsync(id);
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(createUserDTO newUser)
        {
            var user = await _userService.AddUserAsync(newUser);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }
    }
}
