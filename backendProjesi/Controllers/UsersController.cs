using backendProjesi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backendProjesi.Helpers;
using backendProjesi.Services;

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
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            if (_usersContext.Users == null)
            {
                return NotFound();
            }
            return await _usersContext.Users.ToListAsync();
        }

        // Get : api/Users/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            if (_usersContext.Users is null)
            {
                return NotFound();
            }
            var users = await _usersContext.Users.FindAsync(id);
            if (users is null)
            {
                return NotFound();
            }
            return users;
        }

        // Post : api/Users
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers(Users users)
        {
            _usersContext.Users.Add(users);
            await _usersContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsers), new { id = users.Id }, users);
        }

        // Put : api/Users/2
        [HttpPut]
        public async Task<ActionResult<Users>> PutUsers(int id, Users users)
        {
            if (id != users.Id)
            {
                return BadRequest();
            }
            _usersContext.Entry(users).State = EntityState.Modified;
            try
            {
                await _usersContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool UsersExists(long id)
        {
            return (_usersContext.Users?.Any(users => users.Id == id)).GetValueOrDefault();
        }

        // Delete : api/Users/2
        [HttpDelete("{id}")]
        public async Task<ActionResult<Users>> DeleteUsers(int id)
        {
            if (_usersContext.Users is null)
            {
                return NotFound();
            }
            var users = await _usersContext.Users.FindAsync(id);
            if (users is null)
            {
                return NotFound();
            }
            _usersContext.Users.Remove(users);
            await _usersContext.SaveChangesAsync();
            return NoContent();
        }        

        [HttpPost("login")]
        public async Task<IActionResult> login(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
    }
}
