namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Cart Item entity in the business domain.
/// </summary>
public sealed class CartItem
{
    /// <summary>
    /// Represents the unique identifier for the user that have the
    /// product(s)/cart item in their cart.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Represents the unique identifier for the product that the cart item contains.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Represents the unique identifier for the product type
    /// of the product that the cart item contains.
    /// </summary>
    public int ProductTypeId { get; set; }

    /// <summary>
    /// Represents the quantity of the product that the cart item contains.
    /// </summary>
    public int Quantity { get; set; } = 1; 
}
