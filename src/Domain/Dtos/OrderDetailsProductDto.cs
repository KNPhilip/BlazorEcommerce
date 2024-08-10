namespace Domain.Dtos;

/// <summary>
/// DTO for representing each product within an order.
/// </summary>
public sealed class OrderDetailsProductDto
{
    public int ProductId { get; set; }
    public required string Title { get; set; }

    /// <summary>
    /// Represents the name of the product type of the product.
    /// </summary>
    public required string ProductType { get; set; }

    public required string ImageUrl { get; set; }
    public List<Image> Images { get; set; } = [];
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; } 
}
