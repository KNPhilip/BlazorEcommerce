using BlazorEcommerce.Domain.Dtos;
using BlazorEcommerce.Domain.Models;
using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Server.Services.AuthService;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.CartService;

public sealed class CartService(EcommerceContext context,
    IAuthService authService) : ICartService
{
    private readonly EcommerceContext _context = context;
    private readonly IAuthService _authService = authService;

    public async Task<ResponseDto<List<CartProductResponseDto>>> GetCartProductsAsync(List<CartItem> cartItems)
    {
        List<CartProductResponseDto> response = [];

        foreach (CartItem item in cartItems)
        {
            Product? product = await _context.Products
                .Where(p => p.Id == item.ProductId)
                .FirstOrDefaultAsync();

            if (product is null)
            {
                continue;
            }

            ProductVariant? productVariant = await _context.ProductVariants
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
        cartItems.ForEach(cartItem => cartItem.UserId = _authService.GetNameIdFromClaims());
        _context.CartItems.AddRange(cartItems);
        await _context.SaveChangesAsync();

        return await GetDbCartItems();
    }

    public async Task<ResponseDto<int>> GetCartItemsCountAsync()
    {
        return ResponseDto<int>.SuccessResponse((await _context.CartItems
            .Where(ci => ci.UserId == _authService.GetNameIdFromClaims())
            .ToListAsync()).Count);
    }

    public async Task<ResponseDto<List<CartProductResponseDto>>> GetDbCartItems(int? userId = null)
    {
        userId ??= _authService.GetNameIdFromClaims();

        return await GetCartProductsAsync(await _context.CartItems
            .Where(ci => ci.UserId == userId).ToListAsync());
    }

    public async Task<ResponseDto<bool>> AddToCart(CartItem cartItem)
    {
        cartItem.UserId = _authService.GetNameIdFromClaims();

        CartItem? sameItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
            ci.ProductTypeId == cartItem.ProductTypeId &&
            ci.UserId == cartItem.UserId);

        if (sameItem is null)
        {
            _context.CartItems.Add(cartItem);
        }
        else
        {
            sameItem.Quantity += cartItem.Quantity;
        }

        await _context.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }

    public async Task<ResponseDto<bool>> UpdateQuantity(CartItem cartItem)
    {
        CartItem? dbCartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
            ci.ProductTypeId == cartItem.ProductTypeId &&
            ci.UserId == _authService.GetNameIdFromClaims());

        if (dbCartItem is null)
        {
            ResponseDto<bool>.ErrorResponse("Cart item does not exist.");
        }

        dbCartItem!.Quantity = cartItem.Quantity;
        await _context.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }

    public async Task<ResponseDto<bool>> RemoveItemFromCart(int productId, int productTypeId)
    {
        CartItem? dbCartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == productId &&
            ci.ProductTypeId == productTypeId &&
            ci.UserId == _authService.GetNameIdFromClaims());

        if (dbCartItem is null)
        {
            return ResponseDto<bool>.ErrorResponse("Cart item does not exist.");
        }

        _context.CartItems.Remove(dbCartItem);
        await _context.SaveChangesAsync();

        return ResponseDto<bool>.SuccessResponse(true);
    }
}
