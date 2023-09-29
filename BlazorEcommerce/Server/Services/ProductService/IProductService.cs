namespace BlazorEcommerce.Server.Services.ProductService
{
    /// <summary>
    /// Interface for all things regarding Product Services.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Gets a list of all the Products from the database.
        /// </summary>
        /// <returns>A list of products on success, or an error in case of failure.</returns>
        Task<ServiceResponse<List<Product>>> GetProductsAsync();

        /// <summary>
        /// Gets a single product from the database with the given ID.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>A product on success, or an error in case of failure.</returns>
        Task<ServiceResponse<Product>> GetProductAsync(int productId);

        /// <summary>
        /// Gets a list of Products from the database within a specified category.
        /// </summary>
        /// <param name="categoryUrl"></param>
        /// <returns>A list of products on success, or an error in case of failure.</returns>
        Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);

        /// <summary>
        /// Recieves a list of products, matching the search text, on the given page.
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="page"></param>
        /// <returns>A list of paginated products on success, or an error in case of failure.</returns>
        Task<ServiceResponse<ProductSearchResultDto>> SearchProductsAsync(string searchText, int page);

        /// <summary>
        /// Recieves search suggestions.
        /// </summary>
        /// <param name="searchText"></param>
        /// <returns>A list of suggested words that matches some of the products in the database, or an error on failure.</returns>
        Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchText);

        /// <summary>
        /// Recieves a list of all products marked as "Featured Products"
        /// </summary>
        /// <returns>A list of all featured products on success, or an error in case of failure.</returns>
        Task<ServiceResponse<List<Product>>> GetFeaturedProductsAsync();

        /// <summary>
        /// Recieves a list of all products (also those marked as invisible)
        /// </summary>
        /// <returns>A list of all admin products on success, or an error in case of failure.</returns>
        Task<ServiceResponse<List<Product>>> GetAdminProductsAsync();

        /// <summary>
        /// Creates a new product with the given input(s)
        /// </summary>
        /// <param name="product"></param>
        /// <returns>The given input on success, or an error in case of failure.</returns>
        Task<ServiceResponse<Product>> CreateProductAsync(Product product);
        
        /// <summary>
        /// Updates the properties of a Product.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>The given input on success, or an error in case of failure.</returns>
        Task<ServiceResponse<Product>> UpdateProductAsync(Product product);

        /// <summary>
        /// Deletes the product with the given ID from the database.
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>True on success, or an error in case of failure.</returns>
        Task<ServiceResponse<bool>> DeleteProductsAsync(int productId);
    }
}