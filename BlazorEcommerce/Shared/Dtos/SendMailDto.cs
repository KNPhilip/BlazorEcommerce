namespace BlazorEcommerce.Shared.Dtos
{
    public class SendMailDto
    {
        public required string ToEmail { get; set; }
        public required string Subject { get; set; }
        public required string HTMLBody { get; set; }
    }
}