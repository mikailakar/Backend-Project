using Microsoft.EntityFrameworkCore;
using backendProjesi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace backendProjesi.Models
{
    public class UsersContext: IdentityDbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            :base(options) { }

        public DbSet<Users> Users { get; set; } = null!;
        public DbSet<Rol> Roles { get; set; } = null!;

        public DbSet<UserWithRoleDto> UsersWithRoles { get; set; }

        public async Task<List<UserWithRoleDto>> GetUsersWithRolesAsync()
        {
            return await UsersWithRoles.FromSqlRaw("EXEC GetUsersWithRoles").ToListAsync();
        }
        public async Task<UserWithRoleDto> GetUserWithRoleByIdAsync(int id)
        {
            var usersWithRoles = await UsersWithRoles.FromSqlRaw("EXEC GetUsersWithRoles").ToListAsync();
            
            return usersWithRoles.FirstOrDefault(x => x.Id == id);
        }
    }
}
