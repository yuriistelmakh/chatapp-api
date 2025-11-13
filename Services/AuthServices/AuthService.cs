using BCrypt.Net;
using ChatApp.Api.DTOs;
using ChatApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApp.Api.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ChatDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ChatDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResult> RegisterAsync(AuthRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return new AuthResult { Success = false, Message = "Username is already taken" };

            var user = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return new AuthResult { Success = true, Token = token };
        }

        public async Task<AuthResult> LoginAsync(AuthRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new AuthResult { Success = false, Message = "Wrong email or password" };
            }

            var token = GenerateJwtToken(user);
            return new AuthResult { Success = true, Token = token };
        }

        private string GenerateJwtToken(User user)
        {
            List<Claim> claims = [
                new Claim("nameid", user.Id.ToString()),
                new Claim("unique_name", user.Username)
            ];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
