using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using matcher.API.Data;
using matcher.API.DTOs;
using matcher.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace matcher.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _config;

    public AuthController(IAuthRepository repo, IConfiguration config)
    {
      _config = config;
      _repo = repo;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDto)
    {
      userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

      if (await _repo.UserExists(userForRegisterDto.Username))
      {
        return BadRequest("Username already exists");
      }

      var userToCreate = new User
      {
        Username = userForRegisterDto.Username
      };

      var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

      return StatusCode(201); // to fix
                              //return CreatedAtRoute()
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
    {
      // checks username and password matches against stored
      var userFromRepo = await _repo.Login(userForLoginDTO.Username.ToLower(), userForLoginDTO.Password);

      // give no indication if a valid username or not
      if (userFromRepo == null)
        return Unauthorized();

      // build up the token
      // token contains two claims (users id and users username)

      var claims = new[]
      {
              new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
              new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

      // purpose is to have the server sign the token that
      // returns
      // create a key
      var key = new SymmetricSecurityKey(Encoding.UTF8
        .GetBytes(_config.GetSection("AppSettings:Token").Value));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return Ok(new
      {
        token = tokenHandler.WriteToken(token)
      });
    }
  }
}