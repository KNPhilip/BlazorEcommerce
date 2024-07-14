using Domain.Dtos;
using Domain.Interfaces;
using Domain.Models;

namespace WebUI.Server.Services.ProductService;

public sealed class ProductUIService(
    IProductService productService) : IProductUIService
{
    public List<Product> Products { get; set; } = [];
    public List<Product> AdminProducts { get; set; } = [];
    public string Message { get; set; } = string.Empty;
    public int CurrentPage { get; set; } = 1;
    public int PageCount { get; set; } = 0;
    public string LastSearchTerm { get; set; } = string.Empty;

    public event Action? OnProductsChanged;

    public async Task<Product> CreateProduct(Product product)
    {
        ResponseDto<Product> result =
            await productService.CreateProductAsync(product);
        return result.Data!;
    }

    public async Task DeleteProduct(Product product)
    {
        await productService.DeleteProductsAsync(product.Id);
    }

    public async Task GetAdminProducts()
    {
        ResponseDto<List<Product>> result = 
            await productService.GetAdminProductsAsync();
        AdminProducts = result.Data ?? [];
        CurrentPage = 1;
        PageCount = 0;
        if (AdminProducts!.Count == 0)
        {
            Message = "No products found.";
        }
    }

    public async Task<Product?> GetProduct(int productId)
    {
        ResponseDto<Product> result = await productService
            .GetProductAsync(productId);
        return result.Data;
    }

    public async Task GetProducts(string? categoryUrl = null)
    {
        ResponseDto<List<Product>> result = categoryUrl is null
            ? await productService.GetFeaturedProductsAsync()
            : await productService.GetProductsByCategoryAsync(categoryUrl);

        if (result is not null)
        {
            Products = result.Data ?? [];
        }

        CurrentPage = 1;
        PageCount = 0;

        if (Products.Count == 0)
            Message = "No products found.";

        OnProductsChanged!.Invoke();
    }

    public async Task<List<string>> GetProductSearchSuggestions(string searchTerm)
    {
        ResponseDto<List<string>> result = await productService
            .GetProductSearchSuggestionsAsync(searchTerm);
        return result.Data ?? [];
    }

    public async Task SearchProducts(string searchTerm, int page)
    {
        LastSearchTerm = searchTerm;

        ResponseDto<ProductSearchResultDto>? result = 
            await productService.SearchProductsAsync(searchTerm, page);

        if (result.Data is not null)
        {
            Products = result.Data.Products;
            CurrentPage = result.Data.CurrentPage;
            PageCount = result.Data.Pages;
        }
        if (Products.Count == 0) 
        {
            Message = "No products found.";
        }
        OnProductsChanged?.Invoke();
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        ResponseDto<Product> result = await productService
            .UpdateProductAsync(product);
        return result.Data;
    }
}
