using backendProjesi.Models;

namespace backendProjesi.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<Users?> GetById(int id);
    }
}
