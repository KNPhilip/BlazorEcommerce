using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.ProductService;

public sealed class ProductService(EcommerceContext context, 
    IHttpContextAccessor httpContextAccessor) : IProductService
{
    private readonly EcommerceContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<ResponseDto<Product>> GetProductAsync(int productId)
    {
        Product? product = null;

        if (_httpContextAccessor.HttpContext!.User.IsInRole("Admin"))
        {
            product = await _context.Products
                .Include(p => p.Variants.Where(v => !v.IsSoftDeleted))
                .ThenInclude(v => v.ProductType)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }
        else
        {
            product = await _context.Products
                .Include(p => p.Variants.Where(v => v.Visible && !v.IsSoftDeleted))
                .ThenInclude(v => v.ProductType)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId && p.Visible);
        }
        if (product is null)
        {
            return ResponseDto<Product>.ErrorResponse("This product does not exist.");
        }
        if (product.IsSoftDeleted)
        {
            return ResponseDto<Product>.ErrorResponse($"We're sorry, but \"{product.Title}\" is not available anymore..");
        }

        return ResponseDto<Product>.SuccessResponse(product);
    }

    public async Task<ResponseDto<List<Product>>> GetProductsAsync()
    {
        List<Product> products = await _context.Products
            .Where(p => p.Visible && !p.IsSoftDeleted)
            .Include(p => p.Variants.Where(v => v.Visible && !v.IsSoftDeleted))
            .Include(p => p.Images)
            .ToListAsync();

        return products.Count > 0
            ? ResponseDto<List<Product>>.SuccessResponse(products)
            : ResponseDto<List<Product>>.ErrorResponse("No products found");
    }

    public async Task<ResponseDto<List<Product>>> GetProductsByCategoryAsync(string categoryUrl)
    {
        List<Product> products = await _context.Products
            .Where(p => p.Visible && !p.IsSoftDeleted && p.Category!.Url
                .ToLower().Equals(categoryUrl.ToLower()))
            .Include(p => p.Variants.Where(p => p.Visible && !p.IsSoftDeleted))
            .Include(p => p.Images)
            .ToListAsync();

        return products.Count > 0
            ? ResponseDto<List<Product>>.SuccessResponse(products)
            : ResponseDto<List<Product>>.ErrorResponse("No products found");
    }

    public async Task<ResponseDto<List<string>>> GetProductSearchSuggestionsAsync(string searchTerm)
    {
        List<Product> products = await FindProductsBySearchTextAsync(searchTerm);
        List<string> result = [];

        foreach (Product product in products)
        {
            if (product.Title.Contains(searchTerm, 
                StringComparison.OrdinalIgnoreCase))
            {
                result.Add(product.Title);
            }

            if (product.Description is not null)
            {
                char[] punctuation = product.Description
                    .Where(char.IsPunctuation)
                    .Distinct().ToArray();
                IEnumerable<string> words = product.Description
                    .Split()
                    .Select(s => s.Trim(punctuation));

                foreach (string word in words)
                {
                    if (word.Contains(searchTerm, 
                        StringComparison.OrdinalIgnoreCase)
                        && !result.Contains(word))
                    {
                        result.Add(word);
                    }
                }
            }
        }

        return ResponseDto<List<string>>.SuccessResponse(result);
    }

    public async Task<ResponseDto<ProductSearchResultDto>> SearchProductsAsync(string searchTerm, int page)
    {
        Single pageResults = 2f;
        double pageCount = Math.Ceiling(
            (await FindProductsBySearchTextAsync(searchTerm)).Count / pageResults);

        List<Product> products = await _context.Products
            .Where(p => p.Visible && !p.IsSoftDeleted 
                && p.Title.Contains(searchTerm, 
                    StringComparison.CurrentCultureIgnoreCase) 
                || p.Visible && !p.IsSoftDeleted 
                && p.Description.Contains(searchTerm, 
                    StringComparison.CurrentCultureIgnoreCase))
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

        return ResponseDto<ProductSearchResultDto>.SuccessResponse(response);
    }

    public async Task<ResponseDto<List<Product>>> GetFeaturedProductsAsync()
    {
        List<Product> response = await _context.Products
            .Where(p => p.Featured && p.Visible && !p.IsSoftDeleted)
            .Include(p => p.Variants.Where(v => v.Visible && !v.IsSoftDeleted))
            .Include(p => p.Images)
            .ToListAsync();

        return response.Count > 0
            ? ResponseDto<List<Product>>.SuccessResponse(response)
            : ResponseDto<List<Product>>.ErrorResponse("No products found");
    }

    public async Task<ResponseDto<List<Product>>> GetAdminProductsAsync()
    {
        List<Product> response = await _context.Products
            .Where(p => !p.IsSoftDeleted)
            .Include(p => p.Variants.Where(v => !v.IsSoftDeleted))
            .ThenInclude(v => v.ProductType)
            .Include(p => p.Images)
            .ToListAsync();

        return response.Count > 0
            ? ResponseDto<List<Product>>.SuccessResponse(response)
            : ResponseDto<List<Product>>.ErrorResponse("No products found");
    }

    public async Task<ResponseDto<Product>> CreateProductAsync(Product product)
    {
        foreach (ProductVariant variant in product.Variants)
        {
            variant.ProductType = null;
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return ResponseDto<Product>.SuccessResponse(product);
    }

    public async Task<ResponseDto<Product>> UpdateProductAsync(Product product)
    {
        Product? dbProduct = await _context.Products
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        if (dbProduct is null)
        {
            return ResponseDto<Product>.ErrorResponse("Product not found.");
        }

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
                dbVariant.IsSoftDeleted = variant.IsSoftDeleted;
            }
        }

        await _context.SaveChangesAsync();

        return ResponseDto<Product>.SuccessResponse(product);
    }

    public async Task<ResponseDto<bool>> DeleteProductsAsync(int productId)
    {
        Product? dbProduct = await _context.Products.FindAsync(productId);

        if (dbProduct is null)
        {
            return ResponseDto<bool>.ErrorResponse("Product not found");
        }

        dbProduct.IsSoftDeleted = true;
        await _context.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }

    private async Task<List<Product>> FindProductsBySearchTextAsync(string searchTerm)
    {
        return await _context.Products
            .Where(p => p.Visible && !p.IsSoftDeleted 
                && p.Title.Contains(searchTerm, 
                    StringComparison.CurrentCultureIgnoreCase) 
                || p.Visible && !p.IsSoftDeleted 
                && p.Description.Contains(searchTerm, 
                    StringComparison.CurrentCultureIgnoreCase))
            .Include(p => p.Variants)
            .ToListAsync(); 
    }
}
