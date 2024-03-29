﻿namespace BlazorEcommerce.Shared.Models
{
    /// <summary>
    /// Represents the User entity in the business domain.
    /// </summary>
    public class User
    {
        #region Properties
        /// <summary>
        /// Represents the unique identifier for the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Represents the hashed password of the user.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// TODO : Finish this implementation of user verification..
        /// </summary>
        public string VerificationToken { get; set; } = string.Empty;

        /// <summary>
        /// TODO : Finish this implementation of user verification..
        /// </summary>
        public DateTime? VerificationTokenExpires { get; set; }

        /// <summary>
        /// Represents the users token to reset their password.
        /// </summary>
        public string PasswordResetToken { get; set; } = string.Empty;

        /// <summary>
        /// Represents the expiry date of the users Password Reset Token.
        /// </summary>
        public DateTime? ResetTokenExpires { get; set; }

        /// <summary>
        /// Represents the date that the user got registered.
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.Now;

        /// <summary>
        /// Represents the users address.
        /// </summary>
        public Address? Address { get; set; }

        /// <summary>
        /// Represents the role of the user (Customer for instance)
        /// </summary>
        public string Role { get; set; } = "Customer"; 
        #endregion
    }
}