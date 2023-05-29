namespace BlazorEcommerce.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public List<Product> Products { get; set; } = new List<Product>();
        public string Message { get; set; } = "Loading products...";

        public event Action OnProductsChanged;

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl is null ? 
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");

            if (result is not null && result.Data is not null)
                Products = result.Data;

            OnProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchTerm)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/search/suggestions/{searchTerm}");

            return result.Data;
        }

        public async Task SearchProducts(string searchTerm)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchTerm}");
            if (result is not null && result.Data is not null)
                Products = result.Data;
            if (Products.Count == 0) Message = "No products found.";
            OnProductsChanged?.Invoke();
        }
    }
}