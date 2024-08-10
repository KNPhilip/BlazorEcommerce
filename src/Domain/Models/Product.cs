namespace Domain.Models;

public sealed class Product : DbEntity
{
    private string title = string.Empty;
    private string description = string.Empty;
    private string imageUrl = string.Empty;
    private int categoryId;

    [Required]
    public string Title 
    { 
        get => title; 
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            title = value;
        }
    }

    public string Description 
    {
        get => description;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            description = value;
        }
    }

    public string ImageUrl 
    {
        get => imageUrl;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            imageUrl = value;
        }
    }

    public List<Image> Images { get; set; } = [];

    public int CategoryId 
    { 
        get => categoryId;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            categoryId = value;
        }
    }

    public bool Featured { get; set; }
    public Category? Category { get; set; }
    public List<ProductVariant> Variants { get; set; } = [];
    public bool Visible { get; set; } = true;

    [NotMapped]
    public bool Editing { get; set; }

    [NotMapped]
    public bool IsNew { get; set; }
}
