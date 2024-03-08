namespace BlazorEcommerce.Domain.Dtos;

/// <summary>
/// Data Transfer Object for representing each item in a cart.
/// </summary>
public sealed class CartProductResponseDto
{
    /// <summary>
    /// Represents the unique identifier for the product in the cart.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Represents the title of the product in the cart.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Represents the unique identifier for the type of product in the cart.
    /// </summary>
    public int ProductTypeId { get; set; }

    /// <summary>
    /// Represents the name of the type of the product in the cart.
    /// </summary>
    public string ProductType { get; set; } = string.Empty;

    /// <summary>
    /// Represents the URL of the image that represents the product in the cart.
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// Represents the price of the product in the cart.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Represents the quantity of the product in the cart.
    /// </summary>
    public int Quantity { get; set; } 
}
