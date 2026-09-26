using Microsoft.EntityFrameworkCore;
using ProductHub.Api.Models;

namespace ProductHub.Api.Data;

public class ProductHubDbContext : DbContext
{
    public ProductHubDbContext(DbContextOptions<ProductHubDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Description)
                .HasMaxLength(1000);

            entity.Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(p => p.SKU)
                .IsUnique();

            entity.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .HasPrecision(18, 2);

            entity.Property(p => p.StockQuantity)
                .IsRequired();

            entity.Property(p => p.Category)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(p => p.IsActive)
                .HasDefaultValue(true);

            entity.Property(p => p.CreatedAt)
                .IsRequired();

            entity.Property(p => p.UpdatedAt)
                .IsRequired(false);
        });
    }
}
