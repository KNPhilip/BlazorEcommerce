namespace BlazorEcommerce.Shared.Dtos
{
    public class TokenValidateDto
    {
        public string? UserEmail { get; set; } = string.Empty;
        public string? ResetToken { get; set; } = string.Empty;
    }
}