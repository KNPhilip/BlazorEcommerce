namespace BlazorEcommerce.Shared.Dtos
{
    public class OrderDetailsProductDto
    {
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string ProductType { get; set; }
        public string ImageUrl { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}