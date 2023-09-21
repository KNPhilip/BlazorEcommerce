﻿namespace BlazorEcommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly EcommerceContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(EcommerceContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            Product product = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                product = await _context.Products
                .Include(p => p.Variants.Where(v => !v.IsDeleted))
                .ThenInclude(v => v.ProductType)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);
            }
            else
            {
                product = await _context.Products
                    .Include(p => p.Variants.Where(v => v.Visible && !v.IsDeleted))
                    .ThenInclude(v => v.ProductType)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == productId && p.Visible);
            }
            if (product is null)
                return new() { Error = "This product does not exist." };
            else if (product.IsDeleted)
                return new() { Error = $"We're sorry, but \"{product.Title}\" is not available anymore.." };

            return ServiceResponse<Product>.SuccessResponse(product);
        }

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

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            List<Product> products = await _context.Products
                .Where(p => p.Visible && !p.IsDeleted && p.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                    .Include(p => p.Variants.Where(p => p.Visible && !p.IsDeleted))
                    .Include(p => p.Images)
                    .ToListAsync();

            return products.Count > 0
                ? ServiceResponse<List<Product>>.SuccessResponse(products)
                : new ServiceResponse<List<Product>> { Error = "No products found" };
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestionsAsync(string searchTerm)
        {
            var products = await FindProductsBySearchTextAsync(searchTerm);
            List<string> result = new List<string>();

            foreach (var product in products)
            {
                if (product.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }

                if (product.Description is not null)
                {
                    var punctuation = product.Description
                        .Where(char.IsPunctuation)
                        .Distinct().ToArray();
                    var words = product.Description
                        .Split()
                        .Select(s => s.Trim(punctuation));

                    foreach (var word in words)
                    {
                        if (word.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }

            return ServiceResponse<List<string>>.SuccessResponse(result);
        }

        public async Task<ServiceResponse<ProductSearchResultDto>> SearchProductsAsync(string searchTerm, int page)
        {
            var pageResults = 2f;
            var pageCount = Math.Ceiling((await FindProductsBySearchTextAsync(searchTerm)).Count / pageResults);
            var products = await _context.Products
                .Where(p => p.Visible && !p.IsDeleted && p.Title.ToLower()
                .Contains(searchTerm.ToLower()) || p.Visible && !p.IsDeleted && p.Description.ToLower()
                .Contains(searchTerm.ToLower()))
                .Include(p => p.Variants)
                .Include(p => p.Images)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToListAsync();

            var response = new ProductSearchResultDto
            {
                Products = products,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return ServiceResponse<ProductSearchResultDto>.SuccessResponse(response);
        }

        public async Task<List<Product>> FindProductsBySearchTextAsync(string searchTerm) =>
            await _context.Products
                .Where(p => p.Visible && !p.IsDeleted && p.Title.ToLower()
                .Contains(searchTerm.ToLower()) || p.Visible && !p.IsDeleted && p.Description.ToLower()
                .Contains(searchTerm.ToLower()))
                .Include(p => p.Variants)
                .ToListAsync();

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

        public async Task<ServiceResponse<Product>> CreateProductsAsync(Product product)
        {
            foreach (var variant in product.Variants)
                variant.ProductType = null;

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return ServiceResponse<Product>.SuccessResponse(product);
        }

        public async Task<ServiceResponse<Product>> UpdateProductAsync(Product product)
        {
            var dbProduct = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (dbProduct is null)
                return new ServiceResponse<Product> { Error = "Product not found." };

            var productImages = dbProduct.Images;
            _context.Images.RemoveRange(productImages);

            dbProduct.Title = product.Title;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;
            dbProduct.Images = product.Images;

            foreach (var variant in product.Variants)
            {
                var dbVariants = await _context.ProductVariants
                    .SingleOrDefaultAsync(v => v.ProductId == variant.ProductId &&
                    v.ProductTypeId == variant.ProductTypeId);
                if (dbVariants is null)
                {
                    variant.ProductType = null;
                    _context.ProductVariants.Add(variant);
                }
                else
                {
                    dbVariants.ProductTypeId = variant.ProductTypeId;
                    dbVariants.Price = variant.Price;
                    dbVariants.OriginalPrice = variant.OriginalPrice;
                    dbVariants.Visible = variant.Visible;
                    dbVariants.IsDeleted = variant.IsDeleted;
                }
            }

            await _context.SaveChangesAsync();

            return ServiceResponse<Product>.SuccessResponse(product);
        }

        public async Task<ServiceResponse<bool>> DeleteProductsAsync(int productId)
        {
            var dbProducts = await _context.Products.FindAsync(productId);
            if (dbProducts is null)
                return new ServiceResponse<bool> { Error = "Product not found" };

            dbProducts.IsDeleted = true;
            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true);
        }
    }
}