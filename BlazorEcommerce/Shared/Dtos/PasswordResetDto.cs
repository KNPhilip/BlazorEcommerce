namespace BlazorEcommerce.Shared.Dtos
{
    /// <summary>
    /// Data Transfer Object for resetting a users password.
    /// </summary>
    public class PasswordResetDto
    {
        #region Properties
        /// <summary>
        /// Represents the email address of the user to have changed their password.
        /// </summary>
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// Represents the Password Reset Token for validating the password reset request.
        /// </summary>
        public string ResetToken { get; set; } = string.Empty;

        /// <summary>
        /// Represents the new password to replace the old one with.
        /// </summary>
        [Required, StringLength(100, ErrorMessage = "Password needs to be at least 6 characters long.", MinimumLength = 6)]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Represents the confirmed version of the new password,
        /// this should always match the password property.
        /// </summary>
        [Compare("NewPassword", ErrorMessage = "The passwords do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty; 
        #endregion
    }
}