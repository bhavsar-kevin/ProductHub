using ProductHub.Api.DTOs;
using ProductHub.Api.Models;

namespace ProductHub.Api.Mappings;

public static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            SKU = product.SKU,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            Category = product.Category,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt
        };
    }

    public static Product ToEntity(this CreateProductRequest request)
    {
        return new Product
        {
            Name = request.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim(),
            SKU = request.SKU.Trim(),
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            Category = request.Category.Trim(),
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static void ApplyUpdates(this Product product, UpdateProductRequest request)
    {
        product.Name = request.Name.Trim();
        product.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description.Trim();
        product.SKU = request.SKU.Trim();
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.Category = request.Category.Trim();
        product.IsActive = request.IsActive;
        product.UpdatedAt = DateTime.UtcNow;
    }
}
