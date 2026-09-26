using ProductHub.Api.DTOs;

namespace ProductHub.Api.Services;

public interface IProductService
{
    Task<PaginatedResult<ProductDto>> GetProductsAsync(ProductQueryParameters parameters, CancellationToken cancellationToken = default);

    Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<ProductDto> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default);

    Task<bool> UpdateProductAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default);

    Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default);
}
