namespace BlazorEcommerce.Domain.Dtos;

public sealed class UserRegisterDto
{
    [Required, EmailAddress]
    public required string Email { get; set; } = string.Empty;

    [Required, StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "The passwords do not match.")]
    public required string ConfirmPassword { get; set; } = string.Empty; 
}
