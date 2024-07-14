namespace Domain.Dtos;

/// <summary>
/// DTO for resetting a users password.
/// </summary>
public sealed class PasswordResetDto
{
    public string UserEmail { get; set; } = string.Empty;

    /// <summary>
    /// Represents the Password Reset Token for validating the password reset request.
    /// </summary>
    public string ResetToken { get; set; } = string.Empty;

    [Required, StringLength(100, ErrorMessage = "Password needs to be at least 6 characters long.", MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;
    [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
    public string ConfirmNewPassword { get; set; } = string.Empty; 
}
