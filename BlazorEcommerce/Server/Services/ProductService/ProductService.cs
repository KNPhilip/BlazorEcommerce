using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly EcommerceContext _context;

        public ProductService(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            var product = await _context.Products
                .Include(p => p.Variants)
                .ThenInclude(v => v.ProductType)
                .FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                response.Success = false;
                response.Message = "This product does not exist.";
            }
            else
            {
                response.Data = product;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await _context.Products
                    .Include(p => p.Variants)
                    .ThenInclude(v => v.ProductType)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await _context.Products
                    .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchTerm)
        {
            var products = await FindProductsBySearchText(searchTerm);
            List<string> result = new List<string>();

            foreach (var product in products)
            {
                if(product.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Title);
                }

                if(product.Description is not null)
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

            return new ServiceResponse<List<string>>
            {
                Data = result
            };
        }

        public async Task<ServiceResponse<List<Product>>> SearchProducts(string searchTerm)
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await FindProductsBySearchText(searchTerm)
            };

            return response;
        }

        public async Task<List<Product>> FindProductsBySearchText(string searchTerm)
        {
            return await _context.Products
                .Where(p => p.Title.ToLower().Contains(searchTerm.ToLower())
                ||
                p.Description.ToLower().Contains(searchTerm.ToLower()))
                .Include(p => p.Variants)
                .ToListAsync();
        }
    }
}