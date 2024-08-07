using backendProjesi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendProjesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersContext _usersContext;
        public UsersController(UsersContext usersContext)
        {
            _usersContext = usersContext;
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

        //Login : api/Login
        [HttpPost("login")]
        public async Task<ActionResult> Login(Users loginUser)
        {
            if (_usersContext.Users == null)
            {
                return NotFound();
            }

            var user = await _usersContext.Users
                .FirstOrDefaultAsync(u => u.Username == loginUser.Username);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            if (user.Password != loginUser.Password)
            {
                return Unauthorized("Invalid username or password.");
            }

            return Ok("Login successful.");
        }
    }
}
