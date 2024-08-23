using backendProjesi.Models;

namespace backendProjesi.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<VMUsers2>> GetAllUsers();
        Task<Users?> GetUserById(int id);
        Task<Users?> AddNewUser(VMUsers userObj);
        Task<Users?> UpdateUser(int id, VMUsers userObj);
        Task<Users?> DeleteUserById(int id);
        Task<Users?> SoftDeleteUserById(int id);
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<List<string>> GetUserRoles(int userId);
        Task<List<UserWithRoleDto>> GetUsersWithRolesAsync();
        Task<UserWithRoleDto> GetUserWithRoleByIdAsync(int id);
        Task<IEnumerable<Users>> GetAllUsersOrderByDateAsync();
    }
}
