using ProductHub.Api.DTOs;
using ProductHub.Api.Models;

namespace ProductHub.Api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(ProductQueryParameters parameters);

    Task<int> CountAsync(ProductQueryParameters parameters);

    Task<Product?> GetByIdAsync(int id);

    Task<Product?> GetBySkuAsync(string sku);

    Task<Product> AddAsync(Product product);

    Task UpdateAsync(Product product);

    Task DeleteAsync(Product product);

    Task<bool> ExistsBySkuAsync(string sku, int? excludeProductId = null);
}
