using backendProjesi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendProjesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : Controller
    {
        private readonly UsersContext _rolContext;
        public RolController(UsersContext rolContext)
        {
            _rolContext = rolContext;
        }

        // Get : api/Rol
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rol>>> GetRol()
        {
            if (_rolContext.Rol == null)
            {
                return NotFound();
            }
            return await _rolContext.Rol.ToListAsync();
        }

        // Get : api/Rol/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            if (_rolContext.Rol is null)
            {
                return NotFound();
            }
            var rol = await _rolContext.Rol.FirstOrDefaultAsync(u => u.uId == id);
            if (rol is null)
            {
                return NotFound();
            }
            return rol;
        }

        // Post : api/Rol
        [HttpPost]
        public async Task<ActionResult<Rol>> PostRol(Rol rol)
        {
            _rolContext.Rol.Add(rol);
            await _rolContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRol), new { id = rol.Id }, rol);
        }

        // Put : api/Rol/2
        [HttpPut]
        public async Task<ActionResult<Rol>> PutRol(int id, Rol rol)
        {
            if (id != rol.Id)
            {
                return BadRequest();
            }
            _rolContext.Entry(rol).State = EntityState.Modified;
            try
            {
                await _rolContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolExists(id)) { return NotFound(); }
                else { throw; }
            }
            return NoContent();
        }

        private bool RolExists(long id)
        {
            return (_rolContext.Rol?.Any(rol => rol.Id == id)).GetValueOrDefault();
        }

        // Delete : api/Rol/2
        [HttpDelete("{id}")]
        public async Task<ActionResult<Rol>> DeleteRol(int id)
        {
            if (_rolContext.Rol is null)
            {
                return NotFound();
            }
            var rol = await _rolContext.Rol.FindAsync(id);
            if (rol is null)
            {
                return NotFound();
            }
            _rolContext.Rol.Remove(rol);
            await _rolContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
