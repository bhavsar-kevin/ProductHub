using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductHub.Api.Controllers;
using ProductHub.Api.DTOs;
using ProductHub.Api.Middleware;
using ProductHub.Api.Services;

namespace ProductHub.Api.Tests;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_WhenNextThrows_ShouldReturnJson500Response()
    {
        var logger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var middleware = new ExceptionHandlingMiddleware(_ => throw new InvalidOperationException("boom"), logger.Object);
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/products";
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var body = await reader.ReadToEndAsync();

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Contains("application/json", context.Response.ContentType, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("An unexpected error occurred", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task InvokeAsync_WhenNextSucceeds_ShouldPassThroughWithoutModifyingResponse()
    {
        var logger = new Mock<ILogger<ExceptionHandlingMiddleware>>();
        var middleware = new ExceptionHandlingMiddleware(context =>
        {
            context.Response.StatusCode = StatusCodes.Status202Accepted;
            return context.Response.WriteAsync("ok");
        }, logger.Object);
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        using var reader = new StreamReader(context.Response.Body);
        var body = await reader.ReadToEndAsync();

        Assert.Equal(StatusCodes.Status202Accepted, context.Response.StatusCode);
        Assert.Equal("ok", body);
    }

}

public class ProductsControllerTests
{
    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ReturnsNotFound()
    {
        var service = new Mock<IProductService>();
        service.Setup(s => s.GetProductByIdAsync(99, It.IsAny<CancellationToken>())).ReturnsAsync((ProductDto?)null);

        var controller = new ProductsController(service.Object);

        var result = await controller.GetProductById(99);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateProduct_WhenSkuAlreadyExists_ReturnsConflict()
    {
        var service = new Mock<IProductService>();
        service.Setup(s => s.CreateProductAsync(It.IsAny<CreateProductRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("A product with SKU 'LAP-123' already exists."));

        var controller = new ProductsController(service.Object);

        var request = new CreateProductRequest
        {
            Name = "Laptop",
            Description = "Gaming laptop",
            SKU = "LAP-123",
            Price = 1299.99m,
            StockQuantity = 10,
            Category = "Electronics",
            IsActive = true
        };

        var result = await controller.CreateProduct(request);

        var conflict = Assert.IsType<ConflictObjectResult>(result.Result);
        Assert.NotNull(conflict.Value);
    }
}
