namespace BlazorEcommerce.Domain.Dtos;

/// <summary>
/// Data Transfer Object for representing order details.
/// </summary>
public sealed class OrderDetailsDto
{
    /// <summary>
    /// Represents the date that the order was made.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Represents the total price of the order.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Represents the list of products ordered.
    /// </summary>
    public List<OrderDetailsProductDto> Products { get; set; } = new(); 
}
