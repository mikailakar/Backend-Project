using backendProjesi.Models;
using backendProjesi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using AutoMapper;

namespace backendProjesi.Implements
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly UsersContext db;
        private readonly IMapper _mapper;

        public UserService(IOptions<AppSettings> appSettings, UsersContext _db, IMapper mapper)
        {
            _appSettings = appSettings.Value;
            db = _db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await db.Users.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<Users?> GetUserById(int id)
        {
            Users userObj = await db.Users.Where(x => x.IsActive == true).FirstOrDefaultAsync(x => x.Id == id);
            if (userObj == null) { 
                return null;
            }
            return userObj;
        }
        public async Task<Users?> AddNewUser(VMUsers userObj)
        {
            Users user = _mapper.Map<Users>(userObj);
            var passwordService = new PasswordHasher<Users>();
            user.Password = passwordService.HashPassword(user, user.Password);
            user.IsActive = true;
            user.InserDate = DateTime.UtcNow;
            await db.Users.AddAsync(user);
            bool isSuccess = await db.SaveChangesAsync() > 0;
            return isSuccess ? user : null;
        }
        public async Task<Users?> UpdateUser(int id, VMUsers userObj)
        {
            var obj = await db.Users.FirstOrDefaultAsync(c => c.Id == id);
            _mapper.Map(userObj, obj);
            if (!string.IsNullOrEmpty(userObj.Password))
            {
                var passwordService = new PasswordService();
                string hashedPassword = passwordService.HashPassword(userObj.Password);
                obj.Password = hashedPassword;
            }
            db.Users.Update(obj);
            bool isSuccess = await db.SaveChangesAsync() > 0;
            return isSuccess ? obj : null;
        }
        public async Task<Users?> DeleteUserById(int id)
        {
            bool isSuccess = false;
            var userObj = await db.Users.FirstOrDefaultAsync(c => c.Id == id);
            if (userObj != null)
            {
                db.Users.Remove(userObj);
            }
            isSuccess = await db.SaveChangesAsync() > 0;
            return isSuccess ? userObj : null;
        }
        public async Task<Users?> SoftDeleteUserById(int id)
        {
            bool isSuccess = false;
            var obj = await db.Users.FirstOrDefaultAsync(c => c.Id == id);
            if (obj != null)
            {
                obj.IsActive = false;
                db.Users.Update(obj);
                isSuccess = await db.SaveChangesAsync() > 0;
            }
            return isSuccess ? obj : null;
        }
        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var storedHashedPassword = await db.Users.Where(u => u.Email == model.Email).Select(r => r.Password).SingleOrDefaultAsync();
            if (storedHashedPassword == null)
                return null;
            var passwordService = new PasswordService();
            bool isPasswordValid = passwordService.VerifyPassword(storedHashedPassword, model.Password);
            var user = isPasswordValid ? await db.Users.SingleOrDefaultAsync(x => x.Email == model.Email) : null;
            if (user == null) return null;
            var token = await generateJwtToken(user);
            return new AuthenticateResponse(user, token);
        }
        private async Task<string> generateJwtToken(Users user)
        {
            var roles = await db.Roles
                .Where(r => r.UserId == user.Id)
                .Select(r => r.RoleName)
                .ToListAsync();
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = await Task.Run(() =>
            {
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
        public async Task<List<string>> GetUserRoles(int userId)
        {
            return await db.Roles.Where(r => r.UserId == userId).Select(r => r.RoleName).ToListAsync();
        }
        public async Task<List<UserWithRoleDto>> GetUsersWithRolesAsync()
        {
            return await db.UsersWithRoles.FromSqlRaw("EXEC GetUsersWithRoles").ToListAsync();
        }
        public async Task<UserWithRoleDto> GetUserWithRoleByIdAsync(int id)
        {
            var usersWithRoles = await db.UsersWithRoles.FromSqlRaw("EXEC GetUsersWithRoles").ToListAsync();

            return usersWithRoles.FirstOrDefault(c => c.Id == id);
        }
        public async Task<IEnumerable<Users>> GetAllUsersOrderByDateAsync()
        {
            return await db.Users.Where(u => u.InserDate != null).OrderByDescending(u => u.InserDate).ToListAsync();
        }
    }
}
