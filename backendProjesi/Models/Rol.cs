using System.Text.Json.Serialization;

namespace backendProjesi.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? RoleName { get; set; }
        [JsonIgnore]
        public Users User { get; set; }
    }
}
