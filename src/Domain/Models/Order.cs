namespace Domain.Models;

public sealed class Order
{
    private int id;
    private string userId = string.Empty;
    private decimal totalPrice;

    public int Id
    {
        get => id;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            id = value;
        }
    }

    public string UserId
    {
        get => userId;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            userId = value;
        }
    }

    public DateTime OrderDate { get; set; } = DateTime.Now;

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

    public List<OrderItem> OrderItems { get; set; } = [];
}
