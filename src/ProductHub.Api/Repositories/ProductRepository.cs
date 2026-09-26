using Microsoft.EntityFrameworkCore;
using ProductHub.Api.Data;
using ProductHub.Api.DTOs;
using ProductHub.Api.Models;

namespace ProductHub.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductHubDbContext _context;

    public ProductRepository(ProductHubDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(ProductQueryParameters parameters)
    {
        var query = BuildFilteredQuery(parameters);
        return await query
            .OrderBy(p => p.Id)
            .Skip((GetPageNumber(parameters) - 1) * GetPageSize(parameters))
            .Take(GetPageSize(parameters))
            .ToListAsync();
    }

    public async Task<int> CountAsync(ProductQueryParameters parameters)
    {
        var query = BuildFilteredQuery(parameters);
        return await query.CountAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.SKU == sku);
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsBySkuAsync(string sku, int? excludeProductId = null)
    {
        var normalizedSku = sku.Trim();

        return await _context.Products.AnyAsync(p =>
            p.SKU == normalizedSku && (!excludeProductId.HasValue || p.Id != excludeProductId.Value));
    }

    private IQueryable<Product> BuildFilteredQuery(ProductQueryParameters parameters)
    {
        IQueryable<Product> query = _context.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var search = parameters.Search.Trim();
            query = query.Where(p =>
                p.Name.Contains(search) ||
                p.SKU.Contains(search) ||
                (p.Description != null && p.Description.Contains(search)));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Category))
        {
            var category = parameters.Category.Trim();
            query = query.Where(p => p.Category == category);
        }

        if (parameters.IsActive.HasValue)
        {
            query = query.Where(p => p.IsActive == parameters.IsActive.Value);
        }

        return query;
    }

    private static int GetPageNumber(ProductQueryParameters parameters)
    {
        return parameters.PageNumber < 1 ? 1 : parameters.PageNumber;
    }

    private static int GetPageSize(ProductQueryParameters parameters)
    {
        return parameters.PageSize < 1 ? 10 : parameters.PageSize;
    }
}
