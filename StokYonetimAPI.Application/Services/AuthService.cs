using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StokYonetimAPI.Application.DTOs;
using StokYonetimAPI.Application.Interfaces;
using StokYonetimAPI.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StokYonetimAPI.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<string> LoginAsync(LoginDTO dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.passwordHash))
                throw new UnauthorizedAccessException(Error.ErrorMessages.UsernamePasswordError);

            return GenerateToken(user);
        }

        public async Task<string> RegisterAsync(RegisterDTO dto)
        {
            if (await _userRepository.UsernameExistAsync(dto.Username))
                throw new ArgumentException(Error.ErrorMessages.UsernameExist);

            var user = new User
            {
                username = dto.Username,
                passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role
            };

            var created = await _userRepository.CreateAsync(user);
            return GenerateToken(created);
        }  

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.username),
                new Claim(ClaimTypes.Role,           user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}