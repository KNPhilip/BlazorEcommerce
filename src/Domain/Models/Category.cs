namespace Domain.Models;

public sealed class Category : DbEntity
{
    private string name = string.Empty;
    private string url = string.Empty;

    public string Name 
    {
        get => name;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            name = value;
        }
    }

    public string Url 
    {
        get => url;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            url = value;
        }
    }

    public bool Visible { get; set; } = true;

    [NotMapped]
    public bool Editing { get; set; } = false;

    [NotMapped]
    public bool IsNew { get; set; } = false; 
}
