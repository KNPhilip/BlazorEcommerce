namespace Domain.Models;

public sealed class Address : DbEntity
{
    private string userId = string.Empty;
    private string firstName = string.Empty;
    private string lastName = string.Empty;
    private string street = string.Empty;
    private string city = string.Empty;
    private string state = string.Empty;
    private string zip = string.Empty;
    private string country = string.Empty;

    public string UserId 
    {
        get => userId;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            userId = value;
        }
    }

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

    public string FullName { get => FirstName + " " + LastName; }

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
    
    public string State
    {
        get => state;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            state = value;
        }
    }

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
