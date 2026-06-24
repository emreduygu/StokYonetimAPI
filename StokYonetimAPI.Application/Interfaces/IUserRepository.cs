using StokYonetimAPI.Application.PagedResult;
using StokYonetimAPI.Application.Parameters;
using StokYonetimAPI.Domain;

namespace StokYonetimAPI.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> UsernameExistAsync(string username);
        Task<User> CreateAsync(User user);

        Task<PagedResult<User>> GetAllAsync(UserParameters query, int? userId);
        Task<User?> GetByIdAsync(int id);
        Task<User?> UpdateAsync(int id, User updated);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
    }
}
