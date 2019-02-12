using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using hookup.API.Data;
using hookup.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hookup.API.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IHookupRepository _repo;
    private readonly IMapper _mapper;
    public UsersController(IHookupRepository repo, IMapper mapper)
    {
      _mapper = mapper;
      _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
      var users = await _repo.GetUsers();

      var usersToReturn = _mapper.Map<IEnumerable<UserForListDTO>>(users);

      return Ok(usersToReturn);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
      var user = await _repo.GetUser(id);

      var userToReturn = _mapper.Map<UserForDetailedDTO>(user);

      return Ok(userToReturn);
    }
  }
}