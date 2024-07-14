namespace Domain.Dtos;

/// <summary>
/// DTO for paginated search results for products.
/// </summary>
public sealed class ProductSearchResultDto
{
    public List<Product> Products { get; set; } = [];
    public int Pages { get; set; }
    public int CurrentPage { get; set; } 
}
