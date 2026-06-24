using Microsoft.EntityFrameworkCore;
using StokYonetimAPI.Application.Interfaces;
using StokYonetimAPI.Application.PagedResult;
using StokYonetimAPI.Application.Parameters;
using StokYonetimAPI.Domain;

namespace StokYonetimAPI.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) {
            _context = context;
        }
        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        
        public async Task<User?> GetByUsernameAsync(string username) =>
            await _context.Users.FirstOrDefaultAsync(u => u.username == username && !u.IsDeleted);


        public async Task<bool> UsernameExistAsync(string username) =>
            await _context.Users.AnyAsync(u => u.username == username && !u.IsDeleted);

        public async Task<PagedResult<User>> GetAllAsync(UserParameters query, int? userId)
        {
            
            var usersQuery = query.IsDeleted.HasValue
                ? _context.Users.Where(u => u.IsDeleted == query.IsDeleted.Value)
                : _context.Users.Where(u => !u.IsDeleted);

          
            if (userId.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Id == userId.Value);
            }

              if (query.id.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Id == query.id.Value);
            }
            if (!string.IsNullOrWhiteSpace(query.Username))
            {
                usersQuery = usersQuery.Where(u => u.username.Contains(query.Username));
            }
            if (query.Role.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Role == query.Role.Value);
            }

            var totalCount = await usersQuery.CountAsync();

            var items = await usersQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };
        }

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

        public async Task<User?> UpdateAsync(int id, User updated)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (user == null) return null;

            user.username = updated.username;
            user.passwordHash = updated.passwordHash;
            user.Role = updated.Role;

            await _context.SaveChangesAsync();
            return user;
        }

     
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
            if (user == null) return false;

            user.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted);
            if (user == null) return false;

            user.IsDeleted = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
