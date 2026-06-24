using StokYonetimAPI.Application.DTOs;
using StokYonetimAPI.Application.Error;
using StokYonetimAPI.Application.Interfaces;
using StokYonetimAPI.Application.PagedResult;
using StokYonetimAPI.Application.Parameters;
using StokYonetimAPI.Domain;

namespace StokYonetimAPI.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDTO> CreateAsync(CreateProductDTO dto, int userId)
        {
            if (dto.Id <= 0)
                throw new ArgumentException(ErrorMessages.IDNegative);
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException(ErrorMessages.ProductNameEmpty);
            if (dto.StockQuantity < 0)
                throw new ArgumentException(ErrorMessages.StockNegative);
            if (dto.UnitPrice < 0)
                throw new ArgumentException(ErrorMessages.PriceNegative);

            var entity = new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                StockQuantity = dto.StockQuantity,
                UnitPrice = dto.UnitPrice,
                UserID = userId   
            };

            var created = await _repository.CreateAsync(entity);

            return new ProductDTO
            {
                Id = created.Id,
                Name = created.Name,
                StockQuantity = created.StockQuantity,
                UnitPrice = created.UnitPrice,
                UserID = created.UserID
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0) throw new ArgumentException(ErrorMessages.IDNegative);
            return await _repository.DeleteAsync(id);
        }

        public async Task<PagedResult<ProductDTO>> GetAllAsync(ProductParameters query, int? userId)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1;
            if (query.PageSize <= 0) query.PageSize = 10;

            var result = await _repository.GetAllAsync(query, userId);

            return new PagedResult<ProductDTO>
            {
                Items = result.Items.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    StockQuantity = p.StockQuantity,
                    UnitPrice = p.UnitPrice,
                    UserID = p.UserID
                }).ToList(),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };
        }

        public async Task<ProductDTO?> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentException(ErrorMessages.IDNegative);

            var p = await _repository.GetByIdAsync(id);
            if (p == null) return null;

            return new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                StockQuantity = p.StockQuantity,
                UnitPrice = p.UnitPrice,
                UserID = p.UserID
            };
        }

        public async Task<ProductDTO?> UpdateAsync(int id, UpdateProductDTO dto)
        {
            if (id <= 0) throw new ArgumentException(ErrorMessages.IDNegative);

            var entity = new Product
            {
                Name = dto.Name,
                StockQuantity = dto.StockQuantity,
                UnitPrice = dto.UnitPrice
            };

            var updated = await _repository.UpdateAsync(id, entity);
            if (updated == null) return null;

            return new ProductDTO
            {
                Id = updated.Id,
                Name = updated.Name,
                StockQuantity = updated.StockQuantity,
                UnitPrice = updated.UnitPrice,
                UserID = updated.UserID
            };
        }
    }
}