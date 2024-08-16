using backendProjesi.Models;

namespace backendProjesi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<Users?> GetUserById(int id);
        Task<Users?> AddNewUser(Users userObj);
        Task<Users?> UpdateUser(Users userObj);
        Task<Users?> DeleteUserById(int id);
        Task<Users?> SoftDeleteUserById(int id);
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<List<string>> GetUserRoles(int userId);
        Task<List<UserWithRoleDto>> GetUsersWithRolesAsync();
        Task<UserWithRoleDto> GetUserWithRoleByIdAsync(int id);
    }
}
