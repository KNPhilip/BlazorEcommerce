namespace Domain.Models;

public sealed class ProductVariant
{
    private int productId;
    private int productTypeId;
    private decimal price;
    private decimal originalPrice;

    [JsonIgnore]
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

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price 
    {
        get => price;
        set
        {
            Encapsulation.ThrowIfNull(value);

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Price cannot be less than 0.");
            }

            price = value;
        }
    }

    [Column(TypeName = "decimal(18,2)")]
    public decimal OriginalPrice 
    {
        get => originalPrice;
        set
        {
            Encapsulation.ThrowIfNull(value);

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Original price cannot be less than 0.");
            }

            originalPrice = value;
        }
    }

    public bool Visible { get; set; } = true;

    public bool IsSoftDeleted { get; set; }

    [NotMapped]
    public bool Editing { get; set; }

    [NotMapped]
    public bool IsNew { get; set; }
}
