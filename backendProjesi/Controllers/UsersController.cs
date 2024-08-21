using backendProjesi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backendProjesi.Helpers;
using backendProjesi.Implements;
using backendProjesi.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;

namespace backendProjesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersContext _usersContext;
        private IUserService _userService;
        public UsersController(UsersContext usersContext, IUserService userService)
        {
            _usersContext = usersContext;
            _userService = userService;
        }

        // Get : api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAllUsers());
        }

        // Get : api/Users/2
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetUser(int id)
        {
            Users user = await _userService.GetUserById(id);
            if (user == null) {
                return NotFound(new { message = "Not Found!" });
            }
            return Ok(user);
        }

        // Post : api/Users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] VMUsers userObj)
        {
            return Ok(await _userService.AddNewUser(userObj));
        }

        // Put : api/Users/2
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] VMUsers userObj)
        {
            Users user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "User Not Found!" });
            }
            return Ok(await _userService.UpdateUser(id, userObj));
        }

        // Delete : api/Users/2
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Users>> DeleteUsers(int id)
        {
            Users user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "User Not Found!" });
            }
            return Ok(await _userService.DeleteUserById(id));
        }

        // Delete : api/Users/2
        [HttpDelete("soft/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Users>> SoftDeleteUsers(int id)
        {
            Users user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "User Not Found!" });
            }
            return Ok(await _userService.SoftDeleteUserById(id));
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);
            if (response == null)
                return BadRequest(new { message = "Email or password is incorrect" });
            return Ok(response);
        }

        [HttpGet("DecodeJwtToken")]
        public async Task<IActionResult> DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);            
            var user = jwtToken.Claims.First(x => x.Type == "id");
            var exp = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
            if (user == null){
                throw new Exception("Token does not contain a user ID.");
            }
            var userId = int.Parse(user.Value);
            if (exp == null || !long.TryParse(exp, out var expValue)){
                throw new Exception("Token does not contain a valid expiration time.");
            }
            var res = "UserID: " + userId + "\nExpiration: " + DateTimeOffset.FromUnixTimeSeconds(expValue).DateTime;
            return Ok(res);
        }

        [HttpGet("GetUsersWithRoles")]
        public async Task<ActionResult<IEnumerable<UserWithRoleDto>>> GetUsersWithRoles()
        {
            var usersWithRoles = await _userService.GetUsersWithRolesAsync();
            return Ok(usersWithRoles);
        }

        [HttpGet("GetUserWithRoleById")]
        public async Task<ActionResult<UserWithRoleDto>> GetUserWithRoleById(int id)
        {
            var usersWithRole = await _userService.GetUserWithRoleByIdAsync(id);
            if (usersWithRole == null)
            {
                return NotFound(new { message = "User Not Found!" });
            }
            return Ok(usersWithRole);
        }
        [HttpGet("GetAllUsersOrderByDate")]
        public async Task<IActionResult> GetAllUsersOrderByDate()
        {
            return Ok(await _userService.GetAllUsersOrderByDateAsync());
        }
    }
}
