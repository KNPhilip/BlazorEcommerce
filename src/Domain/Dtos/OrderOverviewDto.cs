namespace Domain.Dtos;

/// <summary>
/// DTO representing the overview of an order.
/// </summary>
public sealed class OrderOverviewDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Represents the name(s) of the product(s) that the order is containing.
    /// </summary>
    public required string Product { get; set; }

    /// <summary>
    /// Represents the image url to represent the order.
    /// </summary>
    public string? ProductImageUrl { get; set; }

    /// <summary>
    /// Represents the list of images of the order. However,
    /// only the first image in this list will be used to represent the order.
    /// </summary>
    public List<Image> Images { get; set; } = []; 
}
