using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Server.Controllers;

public sealed class ProductsController(
    IProductService productService) : ControllerTemplate
{
    private readonly IProductService _productService = productService;

    [HttpGet("admin"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<Product>>> GetAdminProducts() =>
        HandleResult(await _productService.GetAdminProductsAsync());

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> CreateProduct(Product product) =>
        HandleResult(await _productService.CreateProductAsync(product));

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<Product>>> UpdateProduct(Product product) =>
        HandleResult(await _productService.UpdateProductAsync(product));

    [HttpDelete("{productId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<bool>> DeleteProduct(int productId) =>
        HandleResult(await _productService.DeleteProductsAsync(productId));

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAllProducts() =>
        HandleResult(await _productService.GetProductsAsync());

    [HttpGet("{productId}")]
    public async Task<ActionResult<Product>> GetProduct(int productId) =>
        HandleResult(await _productService.GetProductAsync(productId));

    [HttpGet("category/{categoryUrl}")]
    public async Task<ActionResult<List<Product>>> GetProductsByCategory(string categoryUrl) =>
        HandleResult(await _productService.GetProductsByCategoryAsync(categoryUrl));

    [HttpGet("search/{searchTerm}/{page}")]
    public async Task<ActionResult<ProductSearchResultDto>> SearchProducts(string searchTerm, int page = 1) =>
        HandleResult(await _productService.SearchProductsAsync(searchTerm, page));

    [HttpGet("search-suggestions/{searchTerm}")]
    public async Task<ActionResult<List<Product>>> GetProductSearchSuggestions(string searchTerm) =>
        HandleResult(await _productService.GetProductSearchSuggestionsAsync(searchTerm));

    [HttpGet("featured")]
    public async Task<ActionResult<List<Product>>> GetFeaturedProducts() =>
        HandleResult(await _productService.GetFeaturedProductsAsync()); 
}
