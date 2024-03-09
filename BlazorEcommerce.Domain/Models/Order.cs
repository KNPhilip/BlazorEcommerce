namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Order entity in the business domain.
/// </summary>
public sealed class Order
{
    private int id;
    private int userId;
    private decimal totalPrice;

    /// <summary>
    /// Represents the unique identifier for the order.
    /// </summary>
    public int Id
    {
        get => id;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            id = value;
        }
    }

    /// <summary>
    /// Represents the unique identifier of the user who made the order
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
    /// Represents the date that the order was made.
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Represents the total price of an order.
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

    /// <summary>
    /// Represents the list of items in an order.
    /// </summary>
    public List<OrderItem> OrderItems { get; set; } = [];
}
