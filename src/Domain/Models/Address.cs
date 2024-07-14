namespace Domain.Models;

/// <summary>
/// Represents the Address entity in the business domain.
/// </summary>
public sealed class Address : DbEntity
{
    private int userId;
    private string firstName = string.Empty;
    private string lastName = string.Empty;
    private string street = string.Empty;
    private string city = string.Empty;
    private string state = string.Empty;
    private string zip = string.Empty;
    private string country = string.Empty;

    /// <summary>
    /// Represents the unique identifier for the user that lives on the address.
    /// </summary>
    public int UserId 
    {
        get => userId;
        set
        {
            Encapsulation.ThrowIfZeroOrLess(value);
            userId = value;
        }
    }

    /// <summary>
    /// Represents the first name of the person living on the address.
    /// </summary>
    [Required, StringLength(20)]
    public string FirstName 
    {
        get => firstName;
        set
        {
            Encapsulation.ThrowIfStringIsTooLong(value, 20);
            firstName = value;
        }
    }

    /// <summary>
    /// Represents the last name of the person living on the address.
    /// </summary>
    [Required, StringLength(20)]
    public string LastName 
    {
        get => lastName;
        set
        {
            Encapsulation.ThrowIfStringIsTooLong(value, 20);
            lastName = value;
        }
    }

    /// <summary>
    /// Represents the full name of the person living on the address.
    /// </summary>
    public string FullName { get => FirstName + " " + LastName; }

    /// <summary>
    /// Represents the name of the street.
    /// </summary>
    [Required, StringLength(50)]
    public string Street 
    {
        get => street;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            street = value;
        }
    }

    /// <summary>
    /// Represents the name of the city.
    /// </summary>
    [Required, StringLength(50)]
    public string City 
    {
        get => city;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            city = value;
        }
    }

    /// <summary>
    /// Represents the name of the state (optional)
    /// </summary>
    public string State
    {
        get => state;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            state = value;
        }
    }

    /// <summary>
    /// Represents the ZIP Code of the address.
    /// </summary>
    [Required, StringLength(10)]
    public string Zip
    {
        get => zip;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            zip = value;
        }
    }

    /// <summary>
    /// Represents the name of the country the address is in.
    /// </summary>
    [Required, StringLength(30)]
    public string Country
    {
        get => country;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            country = value;
        }
    }
}
