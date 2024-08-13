using backendProjesi.Models;
using backendProjesi.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backendProjesi.Implements
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly UsersContext db;

        public UserService(IOptions<AppSettings> appSettings, UsersContext _db)
        {
            _appSettings = appSettings.Value;
            db = _db;
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
        public async Task<Users?> AddNewUser(Users userObj)
        {
            bool isSuccess = false;
            await db.Users.AddAsync(userObj);
            isSuccess = await db.SaveChangesAsync() > 0;
            return isSuccess ? userObj : null;
        }
        public async Task<Users?> UpdateUser(Users userObj)
        {
            bool isSuccess = false;
            var obj = await db.Users.FirstOrDefaultAsync(c => c.Id == userObj.Id);
            if (obj != null)
            {
                obj.Name = userObj.Name;
                obj.Username = userObj.Username;
                obj.Email = userObj.Email;
                obj.Password = userObj.Password;
                db.Users.Update(obj);
                isSuccess = await db.SaveChangesAsync() > 0;
            }
            return isSuccess ? userObj : null;
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
            var user = await db.Users.SingleOrDefaultAsync(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = await generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }
        private async Task<string> generateJwtToken(Users user)
        {
            var roles = await db.Roles
                .Where(r => r.UserId == user.Id)
                .Select(r => r.RoleName)
                .ToListAsync();

            // Create the claims for the token, including the roles
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //Generate token that is valid for 7 days
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
            return await db.Roles
                .Where(r => r.UserId == userId)
                .Select(r => r.RoleName)
                .ToListAsync();
        }
    }
}
