namespace BlazorEcommerce.Shared.Dtos
{
    public class MailSettingsDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required int Port { get; set; }
        public required string FromEmail { get; set; }
        public required string Host { get; set; }
    }
}