namespace ProductHub.Api.DTOs;

public class ProductQueryParameters
{
    public string? Search { get; set; }

    public string? Category { get; set; }

    public bool? IsActive { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
