namespace BlazorEcommerce.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string VerificationToken { get; set; } = string.Empty;
        public DateTime? VerificationTokenExpires { get; set; }
        public string PasswordResetToken { get; set; } = string.Empty;
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public Address? Address { get; set; }
        public string Role { get; set; } = "Customer";
    }
}