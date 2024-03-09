namespace BlazorEcommerce.Domain.Dtos;

public sealed class UserLoginDto
{
    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}
