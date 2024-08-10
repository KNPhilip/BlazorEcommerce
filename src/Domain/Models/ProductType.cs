namespace Domain.Models;

public sealed class ProductType : DbEntity
{
    private string name = string.Empty;

    public string Name 
    {
        get => name;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            name = value;
        }
    }

    [NotMapped]
    public bool Editing { get; set; }

    [NotMapped]
    public bool IsNew { get; set; }
}
