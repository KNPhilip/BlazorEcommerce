using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required, StringLength(20)]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(20)]
        public string LastName { get; set; } = string.Empty;
        public string FullName { get => FirstName + " " + LastName; }
        [Required, StringLength(50)]
        public string Street { get; set; } = string.Empty;
        [Required, StringLength(50)]
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        [Required, StringLength(10)]
        public string Zip { get; set; } = string.Empty;
        [Required, StringLength(30)]
        public string Country { get; set; } = string.Empty;
    }
}