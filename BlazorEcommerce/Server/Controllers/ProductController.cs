using Azure;
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
            var response = await _productService.GetAdminProductsAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
        {
            var response = await _productService.CreateProductsAsync(product);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> EditProduct(Product product)
        {
            var response = await _productService.UpdateProductAsync(product);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("admin/{productId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int productId)
        {
            var response = await _productService.DeleteProductsAsync(productId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAllProducts()
        {
            var response = await _productService.GetProductsAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
        {
            var response = await _productService.GetProductAsync(productId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl)
        {
            var response = await _productService.GetProductsByCategoryAsync(categoryUrl);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("search/{searchTerm}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProducts(string searchTerm, int page = 1)
        {
            var response = await _productService.SearchProductsAsync(searchTerm, page);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("search/suggestions/{searchTerm}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchTerm)
        {
            var response = await _productService.GetProductSearchSuggestionsAsync(searchTerm);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            var response = await _productService.GetFeaturedProductsAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}