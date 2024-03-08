namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Order entity in the business domain.
/// </summary>
public sealed class Order
{
    /// <summary>
    /// Represents the unique identifier for the order.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Represents the unique identifier of the user who made the order
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Represents the date that the order was made.
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Represents the total price of an order.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Represents the list of items in an order.
    /// </summary>
    public List<OrderItem> OrderItems { get; set; } = []; 
}
