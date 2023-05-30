using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Shared.Dtos
{
    public class ProductSearchResultDto
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}