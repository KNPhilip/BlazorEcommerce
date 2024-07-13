namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Product Type entity in the business domain.
/// </summary>
public sealed class ProductType : DbEntity
{
    private string name = string.Empty;

    /// <summary>
    /// Represents the name of the product type.
    /// </summary>
    public string Name 
    {
        get => name;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            name = value;
        }
    }

    /// <summary>
    /// Represents the status of whether or not the product type is currently being edited. This
    /// property is not mapped/saved to the database, its only there for frontend purposes.
    /// </summary>
    [NotMapped]
    public bool Editing { get; set; }

    /// <summary>
    /// Represents the status of whether this product type is "being created" or not. This
    /// property is not mapped/saved to the database, its only there for frontend purposes.
    /// </summary>
    [NotMapped]
    public bool IsNew { get; set; }
}
