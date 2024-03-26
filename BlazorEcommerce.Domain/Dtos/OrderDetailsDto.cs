namespace BlazorEcommerce.Domain.Dtos;

/// <summary>
/// DTO for representing order details.
/// </summary>
public sealed class OrderDetailsDto
{
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Represents the list of products ordered.
    /// </summary>
    public List<OrderDetailsProductDto> Products { get; set; } = []; 
}
