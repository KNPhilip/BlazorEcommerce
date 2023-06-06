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
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchTerm { get; set; } = string.Empty;
        public List<Product> AdminProducts { get; set; }

        public event Action OnProductsChanged;

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _http.PostAsJsonAsync("api/product/admin", product);
            var newProduct = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<Product>>()).Data;

            return newProduct;
        }

        public async Task DeleteProduct(Product product)
        {
            var result = await
                _http.DeleteAsync($"api/product/admin/{product.Id}");
        }

        public async Task GetAdminProducts()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/admin");
            AdminProducts = result.Data;
            CurrentPage = 1;
            PageCount = 0;
            if (AdminProducts.Count == 0)
                Message = "No products found.";
        }

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }

        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = categoryUrl is null ? 
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");

            if (result is not null && result.Data is not null)
                Products = result.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (Products.Count == 0)
                Message = "No products found.";

            OnProductsChanged.Invoke();
        }

        public async Task<List<string>> GetProductSearchSuggestions(string searchTerm)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/search/suggestions/{searchTerm}");

            return result.Data;
        }

        public async Task SearchProducts(string searchTerm, int page)
        {
            LastSearchTerm = searchTerm;

            var result = await _http
                .GetFromJsonAsync<ServiceResponse<ProductSearchResultDto>>($"api/product/search/{searchTerm}/{page}");
            if (result is not null && result.Data is not null)
            {
                Products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }
            if (Products.Count == 0) Message = "No products found.";
            OnProductsChanged?.Invoke();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = await _http.PutAsJsonAsync($"api/product/admin", product);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
        }
    }
}