using Microsoft.Extensions.Logging;
using Moq;
using ProductHub.Api.DTOs;
using ProductHub.Api.Mappings;
using ProductHub.Api.Models;
using ProductHub.Api.Repositories;
using ProductHub.Api.Services;

namespace ProductHub.Api.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repositoryMock;
    private readonly Mock<ILogger<ProductService>> _loggerMock;
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<ProductService>>();
        _service = new ProductService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        var product = CreateProduct();
        _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        var result = await _service.GetProductByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(product.Id, result!.Id);
        Assert.Equal(product.SKU, result.SKU);
    }

    [Fact]
    public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Product?)null);

        var result = await _service.GetProductByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateProductAsync_ShouldCreateProduct_WhenSkuIsUnique()
    {
        var request = CreateCreateProductRequest();
        var createdProduct = CreateProduct();

        _repositoryMock.Setup(r => r.ExistsBySkuAsync(request.SKU, null)).ReturnsAsync(false);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);

        var result = await _service.CreateProductAsync(request);

        Assert.NotNull(result);
        Assert.Equal(createdProduct.Id, result.Id);
        Assert.Equal(createdProduct.SKU, result.SKU);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public async Task CreateProductAsync_ShouldThrow_WhenSkuIsDuplicate()
    {
        var request = CreateCreateProductRequest();
        _repositoryMock.Setup(r => r.ExistsBySkuAsync(request.SKU, null)).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateProductAsync(request));
    }

    [Fact]
    public async Task CreateProductAsync_ShouldThrow_WhenPriceIsNegative()
    {
        var request = CreateCreateProductRequest();
        request.Price = -10m;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateProductAsync(request));
    }

    [Fact]
    public async Task CreateProductAsync_ShouldThrow_WhenStockIsNegative()
    {
        var request = CreateCreateProductRequest();
        request.StockQuantity = -1;

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateProductAsync(request));
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldUpdateProduct_WhenProductExistsAndSkuIsUnique()
    {
        var existingProduct = CreateProduct();
        var request = new UpdateProductRequest
        {
            Name = "Updated Laptop",
            Description = "Updated description",
            SKU = "LAP-2001",
            Price = 1499.99m,
            StockQuantity = 8,
            Category = "Electronics",
            IsActive = true
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(existingProduct.Id)).ReturnsAsync(existingProduct);
        _repositoryMock.Setup(r => r.ExistsBySkuAsync(request.SKU, existingProduct.Id)).ReturnsAsync(false);
        _repositoryMock.Setup(r => r.UpdateAsync(existingProduct)).Returns(Task.CompletedTask);

        var result = await _service.UpdateProductAsync(existingProduct.Id, request);

        Assert.True(result);
        Assert.Equal("Updated Laptop", existingProduct.Name);
        Assert.Equal("LAP-2001", existingProduct.SKU);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        var request = CreateUpdateProductRequest();
        _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Product?)null);

        var result = await _service.UpdateProductAsync(99, request);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldThrow_WhenSkuConflicts()
    {
        var existingProduct = CreateProduct();
        var request = CreateUpdateProductRequest();

        _repositoryMock.Setup(r => r.GetByIdAsync(existingProduct.Id)).ReturnsAsync(existingProduct);
        _repositoryMock.Setup(r => r.ExistsBySkuAsync(request.SKU, existingProduct.Id)).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateProductAsync(existingProduct.Id, request));
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldThrow_WhenPriceIsNegative()
    {
        var existingProduct = CreateProduct();
        var request = CreateUpdateProductRequest();
        request.Price = -1m;

        _repositoryMock.Setup(r => r.GetByIdAsync(existingProduct.Id)).ReturnsAsync(existingProduct);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateProductAsync(existingProduct.Id, request));
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldThrow_WhenStockIsNegative()
    {
        var existingProduct = CreateProduct();
        var request = CreateUpdateProductRequest();
        request.StockQuantity = -1;

        _repositoryMock.Setup(r => r.GetByIdAsync(existingProduct.Id)).ReturnsAsync(existingProduct);

        await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateProductAsync(existingProduct.Id, request));
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldReturnTrue_WhenProductExists()
    {
        var product = CreateProduct();
        _repositoryMock.Setup(r => r.GetByIdAsync(product.Id)).ReturnsAsync(product);
        _repositoryMock.Setup(r => r.DeleteAsync(product)).Returns(Task.CompletedTask);

        var result = await _service.DeleteProductAsync(product.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteProductAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(123)).ReturnsAsync((Product?)null);

        var result = await _service.DeleteProductAsync(123);

        Assert.False(result);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnPaginatedResult_WhenProductsExist()
    {
        var products = new List<Product> { CreateProduct(), CreateProduct() };
        var parameters = new ProductQueryParameters { PageNumber = 1, PageSize = 10 };
        var result = new PaginatedResult<ProductDto>
        {
            Items = products.Select(p => p.ToDto()).ToList(),
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 2,
            TotalPages = 1
        };

        _repositoryMock.Setup(r => r.GetAllAsync(parameters)).ReturnsAsync(products);
        _repositoryMock.Setup(r => r.CountAsync(parameters)).ReturnsAsync(2);

        var response = await _service.GetProductsAsync(parameters);

        Assert.Equal(2, response.Items.Count);
        Assert.Equal(2, response.TotalCount);
        Assert.Equal(1, response.TotalPages);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnEmptyResult_WhenNoProductsExist()
    {
        var parameters = new ProductQueryParameters { PageNumber = 1, PageSize = 10 };
        _repositoryMock.Setup(r => r.GetAllAsync(parameters)).ReturnsAsync(new List<Product>());
        _repositoryMock.Setup(r => r.CountAsync(parameters)).ReturnsAsync(0);

        var response = await _service.GetProductsAsync(parameters);

        Assert.Empty(response.Items);
        Assert.Equal(0, response.TotalCount);
        Assert.Equal(0, response.TotalPages);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldUsePageParameters_WhenProvided()
    {
        var parameters = new ProductQueryParameters { PageNumber = 2, PageSize = 5 };
        var products = new List<Product> { CreateProduct() };

        _repositoryMock.Setup(r => r.GetAllAsync(parameters)).ReturnsAsync(products);
        _repositoryMock.Setup(r => r.CountAsync(parameters)).ReturnsAsync(1);

        var response = await _service.GetProductsAsync(parameters);

        Assert.Equal(2, response.PageNumber);
        Assert.Equal(5, response.PageSize);
    }

    [Fact]
    public async Task CreateProductAsync_ShouldTrimValues_WhenCreatingProduct()
    {
        var request = CreateCreateProductRequest();
        request.Name = "  Trimmed Laptop  ";
        request.SKU = "  LAP-9999  ";
        request.Category = "  Electronics  ";

        var createdProduct = new Product
        {
            Id = 2,
            Name = "Trimmed Laptop",
            Description = "Gaming laptop",
            SKU = "LAP-9999",
            Price = 1299.99m,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _repositoryMock.Setup(r => r.ExistsBySkuAsync("LAP-9999", null)).ReturnsAsync(false);
        _repositoryMock.Setup(r => r.AddAsync(It.Is<Product>(p => p.Name == "Trimmed Laptop" && p.SKU == "LAP-9999" && p.Category == "Electronics"))).ReturnsAsync(createdProduct);

        var result = await _service.CreateProductAsync(request);

        Assert.Equal("Trimmed Laptop", result.Name);
        Assert.Equal("LAP-9999", result.SKU);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldTrimValues_WhenUpdatingProduct()
    {
        var existingProduct = CreateProduct();
        var request = CreateUpdateProductRequest();
        request.Name = "  Updated Name  ";
        request.SKU = "  LAP-5555  ";
        request.Category = "  Accessories  ";

        _repositoryMock.Setup(r => r.GetByIdAsync(existingProduct.Id)).ReturnsAsync(existingProduct);
        _repositoryMock.Setup(r => r.ExistsBySkuAsync("LAP-5555", existingProduct.Id)).ReturnsAsync(false);
        _repositoryMock.Setup(r => r.UpdateAsync(existingProduct)).Returns(Task.CompletedTask);

        var result = await _service.UpdateProductAsync(existingProduct.Id, request);

        Assert.True(result);
        Assert.Equal("Updated Name", existingProduct.Name);
        Assert.Equal("LAP-5555", existingProduct.SKU);
        Assert.Equal("Accessories", existingProduct.Category);
    }

    private static CreateProductRequest CreateCreateProductRequest()
    {
        return new CreateProductRequest
        {
            Name = "Laptop",
            Description = "Gaming laptop",
            SKU = "LAP-1001",
            Price = 1299.99m,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true
        };
    }

    private static UpdateProductRequest CreateUpdateProductRequest()
    {
        return new UpdateProductRequest
        {
            Name = "Laptop",
            Description = "Gaming laptop",
            SKU = "LAP-1001",
            Price = 1299.99m,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true
        };
    }

    private static Product CreateProduct()
    {
        return new Product
        {
            Id = 1,
            Name = "Laptop",
            Description = "Gaming laptop",
            SKU = "LAP-1001",
            Price = 1299.99m,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
    }
}
