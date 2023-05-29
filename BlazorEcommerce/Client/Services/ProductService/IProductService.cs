namespace BlazorEcommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        List<Product> Products { get; set; }
        Task GetProducts(string categoryUrl = null);
        Task <ServiceResponse<Product>> GetProduct(int productId);
    }
}