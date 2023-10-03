namespace BlazorEcommerce.Shared.Dtos
{
    /// <summary>
    /// Data Transfer Object representing the mail settings / config.
    /// This is needed for the mail feature to work.
    /// </summary>
    public class MailSettingsDto
    {
        #region Properties
        /// <summary>
        /// Represents the Username of the Mail Config.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Represents the Password of the Mail Config.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Represents the Port of the Mail Config.
        /// </summary>
        public required int Port { get; set; }

        /// <summary>
        /// Represents the "Sender Email" of the Mail Config.
        /// </summary>
        public required string FromEmail { get; set; }

        /// <summary>
        /// Represents the Host of the Mail Config.
        /// </summary>
        public required string Host { get; set; } 
        #endregion
    }
}