namespace BlazorEcommerce.Shared
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool Featured { get; set; } = false;
        public Category? Category { get; set; }
        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    }
}