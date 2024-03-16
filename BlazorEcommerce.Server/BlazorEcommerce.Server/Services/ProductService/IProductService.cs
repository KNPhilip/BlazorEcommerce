using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;

namespace BlazorEcommerce.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ResponseDto<List<Product>>> GetProductsAsync();
        Task<ResponseDto<Product>> GetProductAsync(int productId);
        Task<ResponseDto<List<Product>>> GetProductsByCategoryAsync(string categoryUrl);
        Task<ResponseDto<ProductSearchResultDto>> SearchProductsAsync(string searchText, int page);
        Task<ResponseDto<List<string>>> GetProductSearchSuggestionsAsync(string searchText);
        Task<ResponseDto<List<Product>>> GetFeaturedProductsAsync();
        Task<ResponseDto<List<Product>>> GetAdminProductsAsync();
        Task<ResponseDto<Product>> CreateProductAsync(Product product);
        Task<ResponseDto<Product>> UpdateProductAsync(Product product);
        Task<ResponseDto<bool>> DeleteProductsAsync(int productId);
    }
}
