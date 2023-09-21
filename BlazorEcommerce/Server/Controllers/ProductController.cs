using Azure;
using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Server.Controllers
{
    public class ProductController : ControllerTemplate
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts() =>
            HandleResult(await _productService.GetAdminProductsAsync());

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product) =>
            HandleResult(await _productService.CreateProductsAsync(product));

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> EditProduct(Product product) =>
            HandleResult(await _productService.UpdateProductAsync(product));

        [HttpDelete("admin/{productId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int productId) =>
            HandleResult(await _productService.DeleteProductsAsync(productId));

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAllProducts() =>
            HandleResult(await _productService.GetProductsAsync());

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId) =>
            HandleResult(await _productService.GetProductAsync(productId));

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl) =>
            HandleResult(await _productService.GetProductsByCategoryAsync(categoryUrl));

        [HttpGet("search/{searchTerm}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProducts(string searchTerm, int page = 1) =>
            HandleResult(await _productService.SearchProductsAsync(searchTerm, page));

        [HttpGet("search/suggestions/{searchTerm}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchTerm) =>
            HandleResult(await _productService.GetProductSearchSuggestionsAsync(searchTerm));

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts() =>
            HandleResult(await _productService.GetFeaturedProductsAsync());
    }
}