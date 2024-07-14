namespace Domain.Models;

/// <summary>
/// Represents the Cart Item entity in the business domain.
/// </summary>
public sealed class CartItem
{
    private int userId;
    private int productId;
    private int productTypeId;
    private int quantity = 1;

    /// <summary>
    /// Represents the unique identifier for the user that have the
    /// product(s)/cart item in their cart.
    /// </summary>
    public int UserId 
    {
        get => userId;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            userId = value;
        }
    }

    /// <summary>
    /// Represents the unique identifier for the product that the cart item contains.
    /// </summary>
    public int ProductId 
    {
        get => productId;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            productId = value;
        }
    }

    /// <summary>
    /// Represents the unique identifier for the product type
    /// of the product that the cart item contains.
    /// </summary>
    public int ProductTypeId 
    {
        get => productTypeId; 
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            productTypeId = value;
        }
    }

    /// <summary>
    /// Represents the quantity of the product that the cart item contains.
    /// </summary>
    public int Quantity 
    {
        get => quantity;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            quantity = value;
        }
    } 
}
