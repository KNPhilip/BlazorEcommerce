namespace BlazorEcommerce.Shared.Models
{
    public class SendMail
    {
        public required string ToEmail { get; set; }
        public required string Subject { get; set; }
        public required string HTMLBody { get; set; }
    }
}