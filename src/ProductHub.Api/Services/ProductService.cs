using ProductHub.Api.DTOs;
using ProductHub.Api.Mappings;
using ProductHub.Api.Repositories;

namespace ProductHub.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<PaginatedResult<ProductDto>> GetProductsAsync(ProductQueryParameters parameters, CancellationToken cancellationToken = default)
    {
        var pageNumber = parameters.PageNumber < 1 ? 1 : parameters.PageNumber;
        var pageSize = parameters.PageSize < 1 ? 10 : parameters.PageSize;

        var products = await _productRepository.GetAllAsync(parameters);
        var totalCount = await _productRepository.CountAsync(parameters);

        var items = products.Select(p => p.ToDto()).ToList();
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);

        return new PaginatedResult<ProductDto>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }

    public async Task<ProductDto?> GetProductByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id);
        return product?.ToDto();
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        ValidateProduct(request.Price, request.StockQuantity, request.SKU);

        var normalizedSku = request.SKU.Trim();
        if (await _productRepository.ExistsBySkuAsync(normalizedSku))
        {
            throw new InvalidOperationException($"A product with SKU '{normalizedSku}' already exists.");
        }

        var product = request.ToEntity();
        var created = await _productRepository.AddAsync(product);

        _logger.LogInformation("Product created with ID {ProductId} and SKU {Sku}", created.Id, created.SKU);

        return created.ToDto(;
    }

    public async Task<bool> UpdateProductAsync(int id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        ValidateProduct(request.Price, request.StockQuantity, request.SKU);

        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
        {
            return false;
        }

        var normalizedSku = request.SKU.Trim();
        if (await _productRepository.ExistsBySkuAsync(normalizedSku, id))
        {
            throw new InvalidOperationException($"A product with SKU '{normalizedSku}' already exists.");
        }

        existingProduct.ApplyUpdates(request);
        await _productRepository.UpdateAsync(existingProduct);

        _logger.LogInformation("Product updated with ID {ProductId}", existingProduct.Id);
        return true;
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            return false;
        }

        await _productRepository.DeleteAsync(product);
        _logger.LogInformation("Product deleted with ID {ProductId}", product.Id);
        return true;
    }

    private static void ValidateProduct(decimal price, int stockQuantity, string sku)
    {
        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative.", nameof(price));
        }

        if (stockQuantity < 0)
        {
            throw new ArgumentException("Stock quantity cannot be negative.", nameof(stockQuantity));
        }

        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU is required.", nameof(sku));
        }
    }
}
