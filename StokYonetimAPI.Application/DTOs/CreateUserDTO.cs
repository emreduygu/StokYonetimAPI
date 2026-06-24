using StokYonetimAPI.Domain;

namespace StokYonetimAPI.Application.DTOs
{
    public class CreateUserDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
