namespace Domain.Dtos;

public sealed class UserLoginDto
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
