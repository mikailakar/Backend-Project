using System.ComponentModel;

namespace backendProjesi.Models
{
    public class AuthenticateRequest
    {
        [DefaultValue("username")]
        public required string Username { get; set; }

        [DefaultValue("password")]
        public required string Password { get; set; }
    }
}
