namespace BlazorEcommerce.Shared.Dtos
{
    /// <summary>
    /// Data Transfer Object to send emails.
    /// </summary>
    public class SendMailDto
    {
        /// <summary>
        /// Represents the property of the reciever of the mail.
        /// </summary>
        public required string ToEmail { get; set; }

        /// <summary>
        /// Represents the name of the subject of the email.
        /// </summary>
        public required string Subject { get; set; }

        /// <summary>
        /// Represents the body (plain HTML in a string) to be used for the mail.
        /// </summary>
        public required string HTMLBody { get; set; }
    }
}