namespace BlazorEcommerce.Shared.Dtos
{
    public class CartProductResponseDto
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ProductTypeId { get; set; }
        public string ProductType { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}