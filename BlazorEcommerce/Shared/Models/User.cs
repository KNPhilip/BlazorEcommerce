namespace BlazorEcommerce.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public Address Address { get; set; }
        public string Role { get; set; } = "Customer";
    }
}