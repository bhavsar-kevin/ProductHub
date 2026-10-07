namespace ProductHub.Api.DTOs;

public class CreateProductRequest : ProductRequestBase
{
    public CreateProductRequest()
    {
        IsActive = true;
    }
}
