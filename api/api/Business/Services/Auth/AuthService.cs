using api.Business.Interfaces.Auth;
using api.Data.Entities;
using api.Data.EntityFramework;
using api.Models.User;
using api.Models.User.Request;
using api.Models.User.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace api.Business.Services.Auth
{
    public class AuthService(AppDbContext _context, IConfiguration configuration) : IAuthService
    {
        public async Task<TokenRsDto> LoginAsync(LoginUserRqDto rq)
        {
            UserEntity user = await _context.Users.FirstOrDefaultAsync(c => c.UserName == rq.UserName);
            if (user is null)
            {
                return null;
            }
            if (new PasswordHasher<UserEntity>().VerifyHashedPassword(user, user.PasswordHash, rq.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }
            UserDto userDto = new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email, Role = user.Role };
            TokenRsDto response = new TokenRsDto { AccessToken = CreateToken(user), RefreshToken = await GenerateAndSaveRefreshTokenAsync(user), User = userDto };
            return response;
        }

        public async Task<RegisterUserRsDto> RegisterAsync(RegisterUserRqDto rq)
        {
            if (await _context.Users.AnyAsync(c => c.UserName == rq.UserName))
            {
                return null;
            }

            UserEntity user = new UserEntity();

            string hashedPassword = new PasswordHasher<UserEntity>()
                .HashPassword(user, rq.Password);

            user.UserName = rq.UserName;
            user.Email = rq.Email;
            user.PasswordHash = hashedPassword;
            user.Name = rq.Name;
            user.LastName = rq.LastName;
            user.Address = rq.Address;
            user.Role = rq.Role;

            _context.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Error guardando usuario: {ex.Message}");
            }

            return new RegisterUserRsDto { Email = user.Email, UserName = user.UserName, statusMessage = "Register successful" };
        }        

        public async Task<TokenRsDto> RefreshTokenAsync(RefreshTokenRqDto rq)
        {
            UserEntity user = await ValidateRefreshTokenAsync(rq.UserId, rq.RefreshToken);
            if (user is null)
                return null;
            UserDto userDto = new UserDto { Id = user.Id, UserName = user.UserName, Email = user.Email, Role = user.Role };
            TokenRsDto response = new TokenRsDto { AccessToken = CreateToken(user), RefreshToken = await GenerateAndSaveRefreshTokenAsync(user), User= userDto };
            return response;
        }

        private async Task<UserEntity> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            UserEntity user = await _context.Users.FindAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }
            return user;
        }

        private string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshTokenAsync(UserEntity user)
        {
            string refreshtoken = GenerateRefreshToken();
            user.RefreshToken = refreshtoken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _context.SaveChangesAsync();
            return refreshtoken;
        }

        private string CreateToken(UserEntity user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("AppSettings:ExpirationInMinutes")),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        }

        public async Task<UserViewDto> UserInfo(int id)
        {
            UserEntity user = await _context.Users.FindAsync(id);

            if (user is null)
                return null;

            return new UserViewDto { Name = user.Name, LastName = user.LastName, Email = user.Email, Role = user.Role };
        }
    }
}
