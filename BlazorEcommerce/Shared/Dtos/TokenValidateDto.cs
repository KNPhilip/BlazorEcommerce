namespace BlazorEcommerce.Shared.Dtos
{
    /// <summary>
    /// Data Transfer Object for validating a Password Reset Token for a specified user.
    /// </summary>
    public class TokenValidateDto
    {
        #region Properties
        /// <summary>
        /// Represents the email address of the user owning the Password Reset Token.
        /// </summary>
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        /// Represents the Password Reset Token.
        /// </summary>
        public string ResetToken { get; set; } = string.Empty; 
        #endregion
    }
}