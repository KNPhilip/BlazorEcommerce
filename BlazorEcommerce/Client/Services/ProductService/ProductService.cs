namespace BlazorEcommerce.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public List<Product> Products { get; set; } = new();
        public string Message { get; set; } = string.Empty;
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchTerm { get; set; } = string.Empty;
        public List<Product> AdminProducts { get; set; } = new();

        public event Action? OnProductsChanged;

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _http.PostAsJsonAsync("api/v1/products", product);
            Product? newProduct = await result.Content
                .ReadFromJsonAsync<Product>();

            return newProduct!;
        }

        public async Task DeleteProduct(Product product) =>
            await _http.DeleteAsync($"api/v1/products/{product.Id}");

        public async Task GetAdminProducts()
        {
            List<Product> result = await _http.GetFromJsonAsync<List<Product>>("api/v1/products/admin") ?? [];
            AdminProducts = result!;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminProducts!.Count == 0)
                Message = "No products found.";
        }

        public async Task<Product?> GetProduct(int productId) =>
            await _http.GetFromJsonAsync<Product?>($"api/v1/products/{productId}");

        public async Task GetProducts(string? categoryUrl = null)
        {
            List<Product>? result = categoryUrl is null ? 
                await _http.GetFromJsonAsync<List<Product>>("api/v1/products/featured") :
                await _http.GetFromJsonAsync<List<Product>>($"api/v1/products/category/{categoryUrl}");

            if (result is not null)
            {
                Products = result;
            }

            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
                Message = "No products found.";

            OnProductsChanged!.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchTerm)
        {
            List<string> result = await _http.GetFromJsonAsync<List<string>>
                ($"api/v1/products/search-suggestions/{searchTerm}") ?? [];

            return result;
        }

        public async Task SearchProducts(string searchTerm, int page)
        {
            LastSearchTerm = searchTerm;

            ProductSearchResultDto? result = await _http
                .GetFromJsonAsync<ProductSearchResultDto>($"api/v1/products/search/{searchTerm}/{page}");
            if (result is not null)
            {
                Products = result.Products;
                CurrentPage = result.CurrentPage;
                PageCount = result.Pages;
            }
            if (Products.Count == 0) Message = "No products found.";
            OnProductsChanged?.Invoke();
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            var result = await _http.PutAsJsonAsync($"api/v1/products", product);
            return await result.Content.ReadFromJsonAsync<Product>();
        }
    }
}