namespace Domain.Models;

public sealed class Image
{
    private int id;
    private string data = string.Empty;

    public int Id 
    {
        get => id;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            id = value;
        }
    }

    public string Data 
    {
        get => data;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            data = value;
        }
    }
}
