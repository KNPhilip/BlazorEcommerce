namespace BlazorEcommerce.Shared.Dtos
{
    public class OrderOverviewDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Product { get; set; }
        public string ProductImageUrl { get; set; }
        public List<Image> Images { get; set; }
    }
}