using StokYonetimAPI.Application.DTOs;
using StokYonetimAPI.Application.PagedResult;
using StokYonetimAPI.Application.Parameters;

namespace StokYonetimAPI.Application.Interfaces
{
    public interface IUserService
    {
        Task<PagedResult<UserDTO>> GetAllAsync(UserParameters query, int? userId);
        Task<UserDTO?> GetByIdAsync(int id);
        Task<UserDTO> CreateAsync(CreateUserDTO dto);
        Task<UserDTO?> UpdateAsync(int id, UpdateUserDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }
}
