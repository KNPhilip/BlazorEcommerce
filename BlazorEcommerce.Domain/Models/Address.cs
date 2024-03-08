namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the Address entity in the business domain.
/// </summary>
public sealed class Address : DbEntity
{
    /// <summary>
    /// Represents the unique identifier for the user that lives on the address.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Represents the first name of the person living on the address.
    /// </summary>
    [Required, StringLength(20)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Represents the last name of the person living on the address.
    /// </summary>
    [Required, StringLength(20)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Represents the full name of the person living on the address.
    /// </summary>
    public string FullName { get => FirstName + " " + LastName; }

    /// <summary>
    /// Represents the name of the street.
    /// </summary>
    [Required, StringLength(50)]
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Represents the name of the city.
    /// </summary>
    [Required, StringLength(50)]
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Represents the name of the state (optional)
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Represents the ZIP Code of the address.
    /// </summary>
    [Required, StringLength(10)]
    public string Zip { get; set; } = string.Empty;

    /// <summary>
    /// Represents the name of the country the address is in.
    /// </summary>
    [Required, StringLength(30)]
    public string Country { get; set; } = string.Empty; 
}
