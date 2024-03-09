namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Order Item entity in the business domain.
/// </summary>
public sealed class OrderItem
{
    private int orderId;
    private int productId;
    private int productTypeId;
    private int quantity;
    private decimal totalPrice;

    /// <summary>
    /// Represents the order that the order item belongs to.
    /// </summary>
    public required Order Order { get; set; }

    /// <summary>
    /// Represents the unique identifier of the order that the order item belongs to.
    /// </summary>
    public int OrderId
    {
        get => orderId; set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            orderId = value;
        }
    }

    /// <summary>
    /// Represents the product that the order item contains of.
    /// </summary>
    public Product? Product { get; set; }

    /// <summary>
    /// Represents the unique identifier of the product that the order item contains of.
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
    /// Represents the product type of the product within the order item.
    /// </summary>
    public ProductType? ProductType { get; set; }

    /// <summary>
    /// Represents the unique identifier for the product type of the
    /// product within the order item.
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
    /// Represents the quantity of the order item.
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

    /// <summary>
    /// Represents the total price of the order item (price * quantity)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice
    {
        get => totalPrice;
        set
        {
            Encapsulation.ThrowIfNull(value);

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The total price cannot be less than 0.");
            }

            totalPrice = value;
        }
    }
}
