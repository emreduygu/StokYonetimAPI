using StokYonetimAPI.Application.Parameters;
using StokYonetimAPI.Application.PagedResult;
using StokYonetimAPI.Domain;

namespace StokYonetimAPI.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResult<Product>> GetAllAsync(ProductParameters query, int? userId);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(int id, Product updated);
        Task<bool> DeleteAsync(int id);
    }
}