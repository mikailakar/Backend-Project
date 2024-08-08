using Microsoft.EntityFrameworkCore;

namespace backendProjesi.Models
{
    public class UsersContext:DbContext
    {
        public UsersContext(DbContextOptions<UsersContext> options)
            :base(options) { }

        public DbSet<Users> Users { get; set; } = null!;

        public DbSet<Rol> Rol { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = 1,
                    Name = "System",
                    Username = "System",
                    Password = "System",
                }
            );
        }
    }
}
