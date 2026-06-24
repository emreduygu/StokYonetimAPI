using StokYonetimAPI.Application.DTOs;
using StokYonetimAPI.Application.Error;
using StokYonetimAPI.Application.Interfaces;
using StokYonetimAPI.Application.PagedResult;
using StokYonetimAPI.Application.Parameters;
using StokYonetimAPI.Domain;

namespace StokYonetimAPI.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDTO> CreateAsync(CreateUserDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new ArgumentException(ErrorMessages.UsernameEmpty);
            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException(ErrorMessages.PasswordEmpty);
            if (await _repository.UsernameExistAsync(dto.Username))
                throw new ArgumentException(ErrorMessages.UsernameExist);

            var entity = new User
            {
                username = dto.Username,
                passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role
            };

            var created = await _repository.CreateAsync(entity);
            return ToDto(created);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException(ErrorMessages.IDNegative);
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> RestoreAsync(int id)
        {
            if (id <= 0) throw new ArgumentException(ErrorMessages.IDNegative);
            return await _repository.RestoreAsync(id);
        }

        public async Task<PagedResult<UserDTO>> GetAllAsync(UserParameters query, int? userId)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 10;

            var result = await _repository.GetAllAsync(query, userId);

            return new PagedResult<UserDTO>
            {
                Items = result.Items.Select(ToDto).ToList(),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException(ErrorMessages.IDNegative);

            var user = await _repository.GetByIdAsync(id);
            return user == null ? null : ToDto(user);
        }

        public async Task<UserDTO?> UpdateAsync(int id, UpdateUserDTO dto)
        {
            if (id <= 0) throw new ArgumentException(ErrorMessages.IDNegative);

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

     
            if (!string.IsNullOrWhiteSpace(dto.Username) && dto.Username != existing.username)
            {
                if (await _repository.UsernameExistAsync(dto.Username))
                    throw new ArgumentException(ErrorMessages.UsernameExist);
            }

        
            var target = new User
            {
                username = string.IsNullOrWhiteSpace(dto.Username) ? existing.username : dto.Username,
                passwordHash = string.IsNullOrWhiteSpace(dto.Password)
                    ? existing.passwordHash
                    : BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role ?? existing.Role
            };

            var updated = await _repository.UpdateAsync(id, target);
            return updated == null ? null : ToDto(updated);
        }

        private static UserDTO ToDto(User user) => new UserDTO
        {
            Id = user.Id,
            Username = user.username,
            Role = user.Role
        };
    }
}
