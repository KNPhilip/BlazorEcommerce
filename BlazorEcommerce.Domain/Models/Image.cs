namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Image entity in the business domain.
/// </summary>
public sealed class Image
{
    private int id;
    private string data = string.Empty;

    /// <summary>
    /// Represents the unique identifier for the image.
    /// </summary>
    public int Id 
    {
        get => id;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            id = value;
        }
    }

    /// <summary>
    /// Represents the base64 string of the image.
    /// </summary>
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
