namespace backendProjesi.Models
{
    public class UserWithRoleDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
    }
}
