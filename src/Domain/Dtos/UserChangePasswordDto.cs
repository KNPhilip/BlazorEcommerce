namespace Domain.Dtos;

/// <summary>
/// DTO for changing a password.
/// </summary>
public sealed class UserChangePasswordDto
{
    [Required, StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "The passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty; 
}
