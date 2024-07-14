namespace Domain.Models;

/// <summary>
/// Represents the Product Variant entity in the business domain.
/// </summary>
public sealed class ProductVariant
{
    private int productId;
    private int productTypeId;
    private decimal price;
    private decimal originalPrice;

    /// <summary>
    /// Represents the product that the variant belongs to.
    /// </summary>
    [JsonIgnore]
    public Product? Product { get; set; }

    /// <summary>
    /// Represents the unique identifier of the product that the variant belongs to.
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
    /// Represents the product type of the product variant.
    /// </summary>
    public ProductType? ProductType { get; set; }

    /// <summary>
    /// Represents the unique identifier of the product type of the product variant.
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
    /// Represents the price of the product variant.
    /// </summary>
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

    /// <summary>
    /// Represents the original price of the product variant. This is only set when you
    /// want to display a discount. Meaning the price property will be the
    /// discount price, and this will be the old price.
    /// </summary>
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

    /// <summary>
    /// Represents the status of whether the product variant
    /// should be visible for regular users or not.
    /// </summary>
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Represents the status of whether the product variant is "soft" deleted or not. On deletion the
    /// data of old product variants is still saved for a period of time in the database. This is a common 
    /// approach in companies for legal reasons. The data is then deleted after x amount of time.
    /// </summary>
    public bool IsSoftDeleted { get; set; }

    /// <summary>
    /// Represents the status of whether or not the product variant is currently being edited. This
    /// property is not mapped/saved to the database, its only there for frontend purposes.
    /// </summary>
    [NotMapped]
    public bool Editing { get; set; }

    /// <summary>
    /// Represents the status of whether this product variant is "being created" or not. This
    /// property is not mapped/saved to the database, its only there for frontend purposes.
    /// </summary>
    [NotMapped]
    public bool IsNew { get; set; }
}
