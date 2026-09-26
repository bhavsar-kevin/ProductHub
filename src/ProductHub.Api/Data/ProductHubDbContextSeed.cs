using ProductHub.Api.Models;

namespace ProductHub.Api.Data;

public static class ProductHubDbContextSeed
{
    public static void Seed(ProductHubDbContext context)
    {
        if (context.Products.Any())
        {
            return;
        }

        var sampleProducts = new[]
        {
            new Product
            {
                Name = "Laptop",
                Description = "14-inch ultrabook for development work",
                SKU = "LAP-1001",
                Price = 1299.99m,
                StockQuantity = 12,
                Category = "Electronics",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            },
            new Product
            {
                Name = "Wireless Keyboard",
                Description = "Bluetooth mechanical keyboard",
                SKU = "KB-1001",
                Price = 149.99m,
                StockQuantity = 45,
                Category = "Accessories",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            },
            new Product
            {
                Name = "Monitor",
                Description = "27-inch 4K display",
                SKU = "MON-2001",
                Price = 399.99m,
                StockQuantity = 18,
                Category = "Electronics",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            },
            new Product
            {
                Name = "Mouse",
                Description = "Ergonomic wireless mouse",
                SKU = "MOU-3001",
                Price = 59.99m,
                StockQuantity = 60,
                Category = "Accessories",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            },
            new Product
            {
                Name = "USB-C Dock",
                Description = "Multi-port hub for laptops",
                SKU = "DOC-4001",
                Price = 99.99m,
                StockQuantity = 30,
                Category = "Accessories",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            }
        };

        context.Products.AddRange(sampleProducts);
        context.SaveChanges();
    }
}
