using StokYonetimAPI.Domain;

namespace StokYonetimAPI.Application.Parameters
{
    public class UserParameters
    {
        public int? id { get; set; }
        public string? Username { get; set; }
        public UserRole? Role { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public bool? IsDeleted { get; set; }
    }
}
