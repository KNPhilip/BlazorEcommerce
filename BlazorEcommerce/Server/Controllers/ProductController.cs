namespace BlazorEcommerce.Server.Controllers
{
    /// <summary>
    /// Product Controller - Contains all endpoints regarding products.
    /// </summary>
    public class ProductController : ControllerTemplate
    {
        /// <summary>
        /// IProductService instance. This accesses the implementation class of the ProductService through the IoC container.
        /// </summary>
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Endpoint for administrators to get all products, even those marked as invisible.
        /// </summary>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts() =>
            HandleResult(await _productService.GetAdminProductsAsync());

        /// <summary>
        /// Endpoint for administrators to create a new product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product) =>
            HandleResult(await _productService.CreateProductAsync(product));

        /// <summary>
        /// Endpoint for administrators to update a product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> EditProduct(Product product) =>
            HandleResult(await _productService.UpdateProductAsync(product));

        /// <summary>
        /// Endpoint for administrators to delete a product from the database with the given ID.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpDelete("admin/{productId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int productId) =>
            HandleResult(await _productService.DeleteProductsAsync(productId));

        /// <summary>
        /// Endpoint to recieve a list of all available products.
        /// </summary>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAllProducts() =>
            HandleResult(await _productService.GetProductsAsync());

        /// <summary>
        /// Endpoint to get a product with the given ID.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId) =>
            HandleResult(await _productService.GetProductAsync(productId));

        /// <summary>
        /// Endpoint to get a list of products, that is within a specific category.
        /// </summary>
        /// <param name="categoryUrl"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl) =>
            HandleResult(await _productService.GetProductsByCategoryAsync(categoryUrl));

        /// <summary>
        /// Endpoint to get a list of paginated products, that matches the given search text, on the specific page.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="page"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet("search/{searchTerm}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProducts(string searchTerm, int page = 1) =>
            HandleResult(await _productService.SearchProductsAsync(searchTerm, page));

        /// <summary>
        /// Endpoint to recieve search suggestions from a string input.
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet("search/suggestions/{searchTerm}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchTerm) =>
            HandleResult(await _productService.GetProductSearchSuggestionsAsync(searchTerm));

        /// <summary>
        /// Endpoint to get a list of all products marked as "Featured Products"
        /// </summary>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts() =>
            HandleResult(await _productService.GetFeaturedProductsAsync());
    }
}