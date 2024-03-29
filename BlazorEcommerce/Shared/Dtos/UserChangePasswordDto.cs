﻿namespace BlazorEcommerce.Shared.Dtos
{
    /// <summary>
    /// Data Transfer Object for changing a password.
    /// </summary>
    public class UserChangePasswordDto
    {
        #region Properties
        /// <summary>
        /// Represents the given password to change to.
        /// </summary>
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Represents the confirmed version of the new password.
        /// This property should match the other password property.
        /// </summary>
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty; 
        #endregion
    }
}