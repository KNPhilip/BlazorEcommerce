namespace BlazorEcommerce.Server.Services.ProductService
{
    /// <summary>
    /// Implementation class of IProductService.
    /// </summary>
    public class ProductService : IProductService
    {
        /// <summary>
        /// EcommerceContext field. Used to access the database context.
        /// </summary>
        private readonly EcommerceContext _context;
        /// <summary>
        /// IHttpContextAccessor field. Used to access the current HTTP context.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <param name="context">EcommerceContext instance to be passed on to the correct
        /// field, containing the correct implementation through the IoC container.</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor instance to be passed on to the
        /// correct field, containing the correct implementation through the IoC container.</param>
        public ProductService(EcommerceContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Recieves a product from the database matching the given ID.
        /// </summary>
        /// <param name="productId">Represents the given ID of the product to recieve.</param>
        /// <returns>The product, or an appropriate error message in case of failure.</returns>
        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            Product? product = null;

            if (_httpContextAccessor.HttpContext!.User.IsInRole("Admin"))
                product = await _context.Products
                    .Include(p => p.Variants.Where(v => !v.IsDeleted))
                    .ThenInclude(v => v.ProductType)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == productId);
            else 
                product = await _context.Products
                    .Include(p => p.Variants.Where(v => v.Visible && !v.IsDeleted))
                    .ThenInclude(v => v.ProductType)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == productId && p.Visible);
            if (product is null)
                return new() { Error = "This product does not exist." };
            else if (product.IsDeleted)
                return new() { Error = $"We're sorry, but \"{product.Title}\" is not available anymore.." };

            return ServiceResponse<Product>.SuccessResponse(product);
        }

        /// <summary>
        /// Recieves a list of all available products in the database.
        /// </summary>
        /// <returns>A list of products, or an error message on failure.</returns>
        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            List<Product> products = await _context.Products
                .Where(p => p.Visible && !p.IsDeleted)
                .Include(p => p.Variants.Where(v => v.Visible && !v.IsDeleted))
                .Include(p => p.Images)
                .ToListAsync();

            return products.Count > 0
                ? ServiceResponse<List<Product>>.SuccessResponse(products)
                : new ServiceResponse<List<Product>> { Error = "No products found" };
        }

        /// <summary>
        /// Recieves a list of products within the specified category name.
        /// </summary>
        /// <param name="categoryUrl">Represents the given name of the
        /// category URL to recieve products from.</param>
        /// <returns>A list of products or an appropriate error message on failure.</returns>
        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            List<Product> products = await _context.Products
                .Where(p => p.Visible && !p.IsDeleted && p.Category!.Url.ToLower().Equals(categoryUrl.ToLower()))
                    .Include(p => p.Variants.Where(p => p.Visible && !p.IsDeleted))
                    .Include(p => p.Images)
                    .ToListAsync();

            return products.Count > 0
                ? ServiceResponse<List<Product>>.SuccessResponse(products)
                : new ServiceResponse<List<Product>> { Error = "No products found" };
        }

        /// <summary>
        /// Recieves a list of suggested product titles based on search text.
        /// </summary>
        /// <param name="searchTerm">Represents the search text to get suggestions based on.</param>
        /// <returns>A list of strings (product title suggestions)</returns>
        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchTerm)
        {
            List<Product> products = await FindProductsBySearchTextAsync(searchTerm);
            List<string> result = new();

            foreach (Product product in products)
            {
                if (product.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    result.Add(product.Title);

                if (product.Description is not null)
                {
                    char[] punctuation = product.Description
                        .Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    IEnumerable<string> words = product.Description
                        .Split()
                        .Select(s => s.Trim(punctuation));

                    foreach (string word in words)
                        if (word.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) 
                            && !result.Contains(word))
                            result.Add(word);
                }
            }

            return ServiceResponse<List<string>>.SuccessResponse(result);
        }

        /// <summary>
        /// Recieves paginated list of products matching the given search text, on the given page.
        /// </summary>
        /// <param name="searchTerm">Represents the text to search for products.</param>
        /// <param name="page">Represents the number of the page to be returned.</param>
        /// <returns>A list of paginated products</returns>
        public async Task<ServiceResponse<ProductSearchResultDto>> SearchProductsAsync(string searchTerm, int page)
        {
            Single pageResults = 2f;
            double pageCount = Math.Ceiling((await FindProductsBySearchTextAsync(searchTerm)).Count / pageResults);
            List<Product> products = await _context.Products
                .Where(p => p.Visible && !p.IsDeleted && p.Title.ToLower()
                .Contains(searchTerm.ToLower()) || p.Visible && !p.IsDeleted && p.Description.ToLower()
                .Contains(searchTerm.ToLower()))
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            ProductSearchResultDto response = new()
            {
                Products = products,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return ServiceResponse<ProductSearchResultDto>.SuccessResponse(response);
        }

        /// <summary>
        /// Recieves a list of all products marked as "Featured Products"
        /// </summary>
        /// <returns>A list of products on success, or an appropriate error message on failure</returns>
        public async Task<ServiceResponse<List<Product>>> GetFeaturedProductsAsync()
        {
            List<Product> response = await _context.Products
                .Where(p => p.Featured && p.Visible && !p.IsDeleted)
                .Include(p => p.Variants.Where(v => v.Visible && !v.IsDeleted))
                .Include(p => p.Images)
                .ToListAsync();

            return response.Count > 0
                ? ServiceResponse<List<Product>>.SuccessResponse(response)
                : new ServiceResponse<List<Product>> { Error = "No products found" };
        }

        /// <summary>
        /// Recieves a list of all products for admins, meaning also the ones that are marked as invisible.
        /// </summary>
        /// <returns>A list of products on success, or an appropriate error message on failure</returns>
        public async Task<ServiceResponse<List<Product>>> GetAdminProductsAsync()
        {
            List<Product> response = await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Variants.Where(v => !v.IsDeleted))
                .ThenInclude(v => v.ProductType)
                .Include(p => p.Images)
                .ToListAsync();

            return response.Count > 0 
                ? ServiceResponse<List<Product>>.SuccessResponse(response)
                : new ServiceResponse<List<Product>> { Error = "No products found" };
        }

        /// <summary>
        /// Creates a new Product in the database.
        /// </summary>
        /// <param name="product">Represents the given Product to be created.</param>
        /// <returns>The given body on success, or an appropriate error message on failure.</returns>
        public async Task<ServiceResponse<Product>> CreateProductAsync(Product product)
        {
            foreach (ProductVariant variant in product.Variants)
                variant.ProductType = null;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return ServiceResponse<Product>.SuccessResponse(product);
        }

        /// <summary>
        /// Updates a Product with the matching ID, replacing the old body with the given.
        /// </summary>
        /// <param name="product">Represents the given product to be updated (Including the ID)</param>
        /// <returns>The given body on success, or an appropriate error message on failure.</returns>
        public async Task<ServiceResponse<Product>> UpdateProductAsync(Product product)
        {
            Product? dbProduct = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (dbProduct is null)
                return new ServiceResponse<Product> { Error = "Product not found." };

            List<Image> productImages = dbProduct.Images;
            _context.Images.RemoveRange(productImages);

            // TODO Might be wise to add Automapper / Mapster here
            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;
            dbProduct.Images = product.Images;

            foreach (ProductVariant variant in product.Variants)
            {
                ProductVariant? dbVariant = await _context.ProductVariants
                    .SingleOrDefaultAsync(v => v.ProductId == variant.ProductId &&
                    v.ProductTypeId == variant.ProductTypeId);
                if (dbVariant is null)
                {
                    variant.ProductType = null;
                    _context.ProductVariants.Add(variant);
                }
                else
                {
                    // TODO Might be wise to add Automapper / Mapster here
                    dbVariant.ProductTypeId = variant.ProductTypeId;
                    dbVariant.Price = variant.Price;
                    dbVariant.OriginalPrice = variant.OriginalPrice;
                    dbVariant.Visible = variant.Visible;
                    dbVariant.IsDeleted = variant.IsDeleted;
                }
            }

            await _context.SaveChangesAsync();

            return ServiceResponse<Product>.SuccessResponse(product);
        }

        /// <summary>
        /// Deletes the Product with the given ID from the database.
        /// </summary>
        /// <param name="productId">Represents the ID of the product to be deleted from the database.</param>
        /// <returns>True/False depending on the success.</returns>
        public async Task<ServiceResponse<bool>> DeleteProductsAsync(int productId)
        {
            Product? dbProduct = await _context.Products.FindAsync(productId);
            if (dbProduct is null)
                return new ServiceResponse<bool> { Error = "Product not found" };

            dbProduct.IsDeleted = true;
            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true);
        }

        /// <summary>
        /// Extracted method to recieve a list of all products matching the given search text.
        /// </summary>
        /// <param name="searchTerm">Represents the search text to find products.</param>
        /// <returns>A list of products on success, or an appropriate error message on failure</returns>
        private async Task<List<Product>> FindProductsBySearchTextAsync(string searchTerm) =>
            await _context.Products
                .Where(p => p.Visible && !p.IsDeleted && p.Title.ToLower()
                .Contains(searchTerm.ToLower()) || p.Visible && !p.IsDeleted && p.Description.ToLower()
                .Contains(searchTerm.ToLower()))
                .Include(p => p.Variants)
                .ToListAsync();
    }
}