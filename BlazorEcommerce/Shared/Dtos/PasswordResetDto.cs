namespace BlazorEcommerce.Shared.Dtos
{
    public class PasswordResetDto
    {
        public string UserEmail { get; set; } = string.Empty;
        public string ResetToken { get; set; } = string.Empty;
        [Required, StringLength(100, ErrorMessage = "Password needs to be at least 6 characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;
        [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}