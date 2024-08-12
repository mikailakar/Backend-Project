using backendProjesi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backendProjesi.Helpers;
using backendProjesi.Implements;
using backendProjesi.Interfaces;

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
        //[Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetAllUsers());
        }

        // Get : api/Users/2
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) {
                return NotFound();
            }
            return Ok(user);
        }

        // Post : api/Users
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> Post([FromBody] Users userObj)
        {
            userObj.Id = 0;
            return Ok(await _userService.AddNewUser(userObj));
        }

        // Put : api/Users/2
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] Users userObj)
        {
            userObj.Id = id;
            return Ok(await _userService.UpdateUser(userObj));
        }

        // Delete : api/Users/2
        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<ActionResult<Users>> DeleteUsers(int id)
        {
            return Ok(await _userService.DeleteUserById(id));
        }

        // Delete : api/Users/2
        [HttpDelete("soft/{id}")]
        //[Authorize]
        public async Task<ActionResult<Users>> SoftDeleteUsers(int id)
        {
            return Ok(await _userService.SoftDeleteUserById(id));
        }

        /*[HttpPost("login")]
        public async Task<IActionResult> login(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }*/
    }
}
