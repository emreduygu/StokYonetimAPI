using StokYonetimAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace StokYonetimAPI.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTO dto);
        Task<string> LoginAsync(LoginDTO dto);
        
    }
}
