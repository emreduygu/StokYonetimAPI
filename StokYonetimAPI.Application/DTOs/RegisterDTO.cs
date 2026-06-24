using StokYonetimAPI.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace StokYonetimAPI.Application.DTOs
{
    public class RegisterDTO
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role {  get; set; }


    }
}
