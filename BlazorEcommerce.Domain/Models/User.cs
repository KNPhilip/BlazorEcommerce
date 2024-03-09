namespace BlazorEcommerce.Domain.Models;

/// <summary>
/// Represents the User entity in the business domain.
/// </summary>
public sealed class User : DbEntity
{
    private string email = string.Empty;
    private string passwordHash = string.Empty;
    private string verificationToken = string.Empty;
    private string passwordResetToken = string.Empty;
    private string role = "Customer";

    /// <summary>
    /// Represents the email address of the user.
    /// </summary>
    public string Email
    {
        get => email;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            email = value;
        }
    }

    /// <summary>
    /// Represents the hashed password of the user.
    /// </summary>
    public string PasswordHash
    {
        get => passwordHash;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            passwordHash = value;
        }
    }

    /// <summary>
    /// TODO : Finish this implementation of user verification..
    /// </summary>
    public string VerificationToken
    {
        get => verificationToken;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            verificationToken = value;
        }
    }

    /// <summary>
    /// TODO : Finish this implementation of user verification..
    /// </summary>
    public DateTime? VerificationTokenExpires { get; set; }

    /// <summary>
    /// Represents the users token to reset their password.
    /// </summary>
    public string PasswordResetToken
    {
        get => passwordResetToken;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            passwordResetToken = value;
        }
    }

    /// <summary>
    /// Represents the expiry date of the users Password Reset Token.
    /// </summary>
    public DateTime? ResetTokenExpires { get; set; }

    /// <summary>
    /// Represents the users address.
    /// </summary>
    public Address? Address { get; set; }

    /// <summary>
    /// Represents the role of the user (Customer for instance)
    /// </summary>
    public string Role
    {
        get => role;
        set
        {
            Encapsulation.ThrowIfNullOrWhiteSpace(value);
            role = value;
        }
    }
}
