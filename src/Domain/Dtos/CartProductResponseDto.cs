namespace Domain.Dtos;

/// <summary>
/// DTO for representing each item in a cart.
/// </summary>
public sealed class CartProductResponseDto
{
    /// <summary>
    /// Represents the unique identifier for the product in the cart.
    /// </summary>
    public int ProductId { get; set; }

    public string Title { get; set; } = string.Empty;
    public int ProductTypeId { get; set; }

    /// <summary>
    /// Represents the name of the type of the product in the cart.
    /// </summary>
    public string ProductType { get; set; } = string.Empty;

    /// <summary>
    /// Represents the URL of the image that represents the product in the cart.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    public decimal Price { get; set; }
    public int Quantity { get; set; } 
}
