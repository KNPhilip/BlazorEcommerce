namespace BlazorEcommerce.Shared.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool Featured { get; set; } = false;
        public Category? Category { get; set; }
        public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
        public bool Visible { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        [NotMapped]
        public bool Editing { get; set; } = false;
        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}