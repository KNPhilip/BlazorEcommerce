namespace BlazorEcommerce.Shared.Dtos
{
    /// <summary>
    /// Data Transfer Object for paginated search results for products.
    /// </summary>
    public class ProductSearchResultDto
    {
        /// <summary>
        /// Represents the list of products resulting from the search.
        /// </summary>
        public List<Product> Products { get; set; } = new List<Product>();

        /// <summary>
        /// Represents the total number of pages.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Represents the current page number.
        /// </summary>
        public int CurrentPage { get; set; }
    }
}