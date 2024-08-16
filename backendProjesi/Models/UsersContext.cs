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
    }
}
