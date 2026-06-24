using StokYonetimAPI.Domain;

namespace StokYonetimAPI.Application.DTOs
{
    public class UpdateUserDTO
    {
     
        public string? Username { get; set; }
        public string? Password { get; set; }
        public UserRole? Role { get; set; }
    }
}
