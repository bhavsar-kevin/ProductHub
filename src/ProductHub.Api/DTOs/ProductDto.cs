using ProductHub.Api.Models;

namespace ProductHub.Api.DTOs;

public class ProductDto : ProductBase
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
