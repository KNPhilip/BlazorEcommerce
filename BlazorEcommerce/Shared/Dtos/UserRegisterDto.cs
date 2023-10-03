namespace BlazorEcommerce.Shared.Dtos
{
    /// <summary>
    /// Data Transfer Object for registering an account.
    /// </summary>
    public class UserRegisterDto
    {
        #region Properties
        /// <summary>
        /// Represents the given to-be registered email.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Represents the given password.
        /// </summary>
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Represents the confirmed version of the password,
        /// this should match the password property.
        /// </summary>
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty; 
        #endregion
    }
}