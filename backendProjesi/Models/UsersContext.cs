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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasOne(u => u.Roles)
                .WithOne(r => r.User)
                .HasForeignKey<Rol>(r => r.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
