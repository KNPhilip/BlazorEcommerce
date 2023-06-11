using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
        {
            var result = await _productService.GetAdminProductsAsync();
            return Ok(result);
        }

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
        {
            var result = await _productService.CreateProductsAsync(product);
            return Ok(result);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> EditProduct(Product product)
        {
            var result = await _productService.UpdateProductsAsync(product);
            return Ok(result);
        }

        [HttpDelete("admin/{productId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int productId)
        {
            var result = await _productService.DeleteProductsAsync(productId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAllProducts()
        {
            var result = await _productService.GetProductsAsync();
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
        {
            var result = await _productService.GetProductAsync(productId);
            return Ok(result);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
        {
            var result = await _productService.GetProductsByCategoryAsync(categoryUrl);
            return Ok(result);
        }

        [HttpGet("search/{searchTerm}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProducts(string searchTerm, int page = 1)
        {
            var result = await _productService.SearchProductsAsync(searchTerm, page);
            return Ok(result);
        }

        [HttpGet("search/suggestions/{searchTerm}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchTerm)
        {
            var result = await _productService.GetProductSearchSuggestionsAsync(searchTerm);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            var result = await _productService.GetFeaturedProductsAsync();
            return Ok(result);
        }
    }
}