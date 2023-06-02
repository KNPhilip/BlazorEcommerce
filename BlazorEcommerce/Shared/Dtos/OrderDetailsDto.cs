namespace BlazorEcommerce.Shared.Dtos
{
    public class OrderDetailsDto
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderDetailsProductDto> Products { get; set; }
    }
}