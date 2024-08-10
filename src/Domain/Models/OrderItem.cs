namespace Domain.Models;

public sealed class OrderItem
{
    private int orderId;
    private int productId;
    private int productTypeId;
    private int quantity;
    private decimal totalPrice;

    public required Order Order { get; set; }

    public int OrderId
    {
        get => orderId; set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            orderId = value;
        }
    }

    public Product? Product { get; set; }

    public int ProductId
    {
        get => productId;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            productId = value;
        }
    }

    public ProductType? ProductType { get; set; }

    public int ProductTypeId
    {
        get => productTypeId;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            productTypeId = value;
        }
    }

    public int Quantity
    {
        get => quantity;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            quantity = value;
        }
    }

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
