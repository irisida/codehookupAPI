using System.Threading.Tasks;
using matcher.API.Data;
using matcher.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace matcher.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string password)
        {
            // validation of request
            username = username.ToLower();

            if (await _repo.UserExists(username))
            {
                return BadRequest("Username already exists");
            }

            var userToCreate = new User
            {
                Username = username
            };

            var createdUser = await _repo.Register(userToCreate, password);

            return StatusCode(201); // to fix
            //return CreatedAtRoute()
        }
    }
}