namespace BlazorEcommerce.Shared.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get => FirstName + " " + LastName; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public Address Address { get; set; }
    }
}