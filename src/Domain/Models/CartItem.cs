namespace Domain.Models;

public sealed class CartItem
{
    public string UserId { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public int ProductTypeId { get; set; }
    public int Quantity { get; set; }
}
