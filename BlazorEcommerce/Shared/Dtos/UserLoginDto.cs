using System.ComponentModel.DataAnnotations;

namespace BlazorEcommerce.Shared.Dtos
{
    public class UserLoginDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}