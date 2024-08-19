using System.ComponentModel;

namespace backendProjesi.Models
{
    public class AuthenticateRequest
    {
        [DefaultValue("email")]
        public required string Email { get; set; }

        [DefaultValue("password")]
        public required string Password { get; set; }
    }
}
