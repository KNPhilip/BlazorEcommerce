namespace BlazorEcommerce.Client.Services.ProductService
{
    public interface IProductService
    {
        event Action OnProductsChanged;
        List<Product> Products { get; set; }
        List<Product> AdminProducts { get; set; }
        string Message { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchTerm { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProduct(int productId);
        Task SearchProducts(string searchTerm, int page);
        Task<List<string>> GetProductSearchSuggestions(string searchTerm);
        Task GetAdminProducts();
        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(Product product);
    }
}