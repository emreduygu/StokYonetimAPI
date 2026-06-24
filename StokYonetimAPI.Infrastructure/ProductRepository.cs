using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokYonetimAPI.Application.Interfaces;
using StokYonetimAPI.Application.PagedResult;
using StokYonetimAPI.Application.Parameters;
using StokYonetimAPI.Domain;

namespace StokYonetimAPI.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
       
        public ProductRepository(AppDbContext context) { _context = context; }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Product>> GetAllAsync(ProductParameters query, int? userId)
        {
            var productsQuery = _context.Products.AsQueryable();

            if (query.id.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Id == query.id.Value);
            }
            if (userId.HasValue)
            {   
                productsQuery = productsQuery.Where(p => p.UserID == userId.Value);
            }
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(query.Name));
            }

            if (query.StockQuantity.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.StockQuantity == query.StockQuantity.Value);
            }

            if (query.minStockQuantity.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.StockQuantity >= query.minStockQuantity.Value);
            }

            if (query.maxStockQuantity.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.StockQuantity <= query.maxStockQuantity.Value);
            }

            if (query.Price.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.UnitPrice == query.Price.Value);
            }

            if (query.minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.UnitPrice >= query.minPrice.Value);
            }

            if (query.maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.UnitPrice <= query.maxPrice.Value);
            }
            

            var totalCount = await productsQuery.CountAsync();

            var items = await productsQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Product>
            {
                Items = items,
                PageNumber = query.PageNumber,
                PageSize = (int)query.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product?> UpdateAsync(int id, Product updated)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            product.Name = updated.Name;
            product.StockQuantity = updated.StockQuantity;
            product.UnitPrice = updated.UnitPrice;
           
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
