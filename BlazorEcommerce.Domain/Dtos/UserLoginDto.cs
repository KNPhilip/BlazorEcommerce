namespace BlazorEcommerce.Domain.Dtos;

/// <summary>
/// Data Transfer Object to login to an account.
/// </summary>
public sealed class UserLoginDto
{
    /// <summary>
    /// Represents the given email to login to.
    /// </summary>
    [Required]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Represents the given password for the login.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty; 
}
