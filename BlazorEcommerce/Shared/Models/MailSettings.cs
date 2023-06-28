namespace BlazorEcommerce.Shared.Models
{
    public class MailSettings
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required int Port { get; set; }
        public required string FromEmail { get; set; }
        public required string Host { get; set; }
    }
}