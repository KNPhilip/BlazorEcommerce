using Domain.Dtos;
using Domain.Models;
using WebUI.Server.Data;
using WebUI.Server.Services.AuthService;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Server.Services.CartService;

public sealed class CartService(IDbContextFactory<EcommerceContext> contextFactory,
    IAuthService authService) : ICartService
{
    private readonly IDbContextFactory<EcommerceContext> _contextFactory = contextFactory;
    private readonly IAuthService _authService = authService;

    public async Task<ResponseDto<List<CartProductResponseDto>>> GetCartProductsAsync(List<CartItem> cartItems)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        List<CartProductResponseDto> response = [];

        foreach (CartItem item in cartItems)
        {
            Product? product = await dbContext.Products
                .Where(p => p.Id == item.ProductId)
                .FirstOrDefaultAsync();

            if (product is null)
            {
                continue;
            }

            ProductVariant? productVariant = await dbContext.ProductVariants
                .Where(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId)
                .Include(v => v.ProductType)
                .FirstOrDefaultAsync();

            if (productVariant is null)
            {
                continue;
            }

            CartProductResponseDto cartProduct = new()
            {
                // TODO Might be wise to add AutoMapper / Mapster here
                ProductId = product.Id,
                Title = product.Title,
                ImageUrl = product.ImageUrl,
                Price = productVariant.Price,
                ProductType = productVariant.ProductType!.Name,
                ProductTypeId = productVariant.ProductTypeId,
                Quantity = item.Quantity
            };

            response.Add(cartProduct);
        }

        return response.Count > 0
            ? ResponseDto<List<CartProductResponseDto>>.SuccessResponse(response)
            : ResponseDto<List<CartProductResponseDto>>.ErrorResponse("No products found.");
    }

    public async Task<ResponseDto<List<CartProductResponseDto>>> StoreCartItemsAsync(List<CartItem> cartItems)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        cartItems.ForEach(cartItem => cartItem.UserId = /*_authService.GetNameIdFromClaims()*/Guid.NewGuid());
        dbContext.CartItems.AddRange(cartItems);
        await dbContext.SaveChangesAsync();

        return await GetDbCartItems();
    }

    public async Task<ResponseDto<int>> GetCartItemsCountAsync()
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        return ResponseDto<int>.SuccessResponse((await dbContext.CartItems
            .Where(ci => ci.UserId == /*_authService.GetNameIdFromClaims()*/Guid.NewGuid())
            .ToListAsync()).Count);
    }

    public async Task<ResponseDto<List<CartProductResponseDto>>> GetDbCartItems(int? userId = null)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        userId ??= _authService.GetNameIdFromClaims();

        return await GetCartProductsAsync(await dbContext.CartItems
            .Where(ci => ci.UserId == /*userId*/Guid.NewGuid()).ToListAsync());
    }

    public async Task<ResponseDto<bool>> AddToCart(CartItem cartItem)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        cartItem.UserId = /*_authService.GetNameIdFromClaims()*/Guid.NewGuid();

        CartItem? sameItem = await dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
            ci.ProductTypeId == cartItem.ProductTypeId &&
            ci.UserId == cartItem.UserId);

        if (sameItem is null)
        {
            dbContext.CartItems.Add(cartItem);
        }
        else
        {
            sameItem.Quantity += cartItem.Quantity;
        }

        await dbContext.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }

    public async Task<ResponseDto<bool>> UpdateQuantity(CartItem cartItem)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        CartItem? dbCartItem = await dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
            ci.ProductTypeId == cartItem.ProductTypeId &&
            ci.UserId == /*_authService.GetNameIdFromClaims()*/Guid.NewGuid());

        if (dbCartItem is null)
        {
            ResponseDto<bool>.ErrorResponse("Cart item does not exist.");
        }

        dbCartItem!.Quantity = cartItem.Quantity;
        await dbContext.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }

    public async Task<ResponseDto<bool>> RemoveItemFromCart(int productId, int productTypeId)
    {
        using EcommerceContext dbContext = _contextFactory.CreateDbContext();

        CartItem? dbCartItem = await dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == productId &&
            ci.ProductTypeId == productTypeId &&
            ci.UserId == /*_authService.GetNameIdFromClaims()*/Guid.NewGuid());

        if (dbCartItem is null)
        {
            return ResponseDto<bool>.ErrorResponse("Cart item does not exist.");
        }

        dbContext.CartItems.Remove(dbCartItem);
        await dbContext.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }
}
