using StokYonetimAPI.Application.DTOs;
using StokYonetimAPI.Application.PagedResult;
using StokYonetimAPI.Application.Parameters;

namespace StokYonetimAPI.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResult<ProductDTO>> GetAllAsync(ProductParameters query, int? userId);
        Task<ProductDTO?> GetByIdAsync(int id);
        Task<ProductDTO> CreateAsync(CreateProductDTO dto, int userId);  
        Task<ProductDTO?> UpdateAsync(int id, UpdateProductDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}