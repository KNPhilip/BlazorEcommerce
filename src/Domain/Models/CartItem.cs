namespace Domain.Models;

public sealed class CartItem
{
    public Guid UserId { get; set; }
    public int ProductId { get; set; }
    public int ProductTypeId { get; set; }
    public int Quantity { get; set; }
}
