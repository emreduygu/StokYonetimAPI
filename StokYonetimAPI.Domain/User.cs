using System;
using System.Collections.Generic;
using System.Text;

namespace StokYonetimAPI.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string username { get; set;} =string.Empty;
        public string passwordHash { get; set;} = string.Empty;
        public UserRole Role { get; set; }

   


        public List<Product> Products { get; set; } = new();
        public bool IsDeleted { get; set; } 
    }
}
