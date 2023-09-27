namespace BlazorEcommerce.Server.Controllers
{
    /// <summary>
    /// Product Controller - Contains all endpoints regarding products.
    /// </summary>
    public class ProductController : ControllerTemplate
    {
        /// <summary>
        /// IProductService field. Used to access the Product Services.
        /// </summary>
        private readonly IProductService _productService;

        /// <param name="productService">IProductService instance to be passed on to the
        /// field, containing the correct implementation class through the IoC container.</param>
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
        /// <param name="product">Represents the given product to be created in the database.</param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product) =>
            HandleResult(await _productService.CreateProductAsync(product));

        /// <summary>
        /// Endpoint for administrators to update a product.
        /// </summary>
        /// <param name="product">Represents the given product to be updated (ID must be included)</param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> UpdateProduct(Product product) =>
            HandleResult(await _productService.UpdateProductAsync(product));

        /// <summary>
        /// Endpoint for administrators to delete a product from the database with the given ID.
        /// </summary>
        /// <param name="productId">Represents the ID of the product to be deleted from the database.</param>
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
        /// <param name="productId">Represents the ID of the product to be recieved.</param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId) =>
            HandleResult(await _productService.GetProductAsync(productId));

        /// <summary>
        /// Endpoint to get a list of products, that is within a specific category.
        /// </summary>
        /// <param name="categoryUrl">Represents the name of the category URL to get products within.</param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategory(string categoryUrl) =>
            HandleResult(await _productService.GetProductsByCategoryAsync(categoryUrl));

        /// <summary>
        /// Endpoint to get a list of paginated products, that matches the given search text, on the specific page.
        /// </summary>
        /// <param name="searchTerm">Represents the search text to recieve products.</param>
        /// <param name="page">Represents the page number to recieve products from.</param>
        /// <returns>Appropriate status code and either the data or an error depending on the response.</returns>
        [HttpGet("search/{searchTerm}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDto>>> SearchProducts(string searchTerm, int page = 1) =>
            HandleResult(await _productService.SearchProductsAsync(searchTerm, page));

        /// <summary>
        /// Endpoint to recieve search suggestions from a string input.
        /// </summary>
        /// <param name="searchTerm">Represents the search text to recieve product suggestions.</param>
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