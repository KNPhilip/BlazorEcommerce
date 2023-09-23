﻿namespace BlazorEcommerce.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly EcommerceContext _context;
        private readonly IAuthService _authService;

        public CartService(EcommerceContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<ServiceResponse<List<CartProductResponseDto>>> GetCartProductsAsync(List<CartItem> cartItems)
        {
            List<CartProductResponseDto> response = new();

            foreach (var item in cartItems)
            {
                Product? product = await _context.Products
                    .Where(p => p.Id == item.ProductId)
                    .FirstOrDefaultAsync();

                if (product is null)
                    continue;

                ProductVariant? productVariant = await _context.ProductVariants
                    .Where(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId)
                    .Include(v => v.ProductType)
                    .FirstOrDefaultAsync();

                if (productVariant is null)
                    continue;

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
                ? ServiceResponse<List<CartProductResponseDto>>.SuccessResponse(response) 
                : new ServiceResponse<List<CartProductResponseDto>> { Error = "No products found." };
        }

        public async Task<ServiceResponse<List<CartProductResponseDto>>> StoreCartItemsAsync(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = _authService.GetNameIdFromClaims());
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetDbCartItems();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCountAsync() =>
            ServiceResponse<int>.SuccessResponse((await _context.CartItems
                .Where(ci => ci.UserId == _authService.GetNameIdFromClaims())
                .ToListAsync()).Count);

        public async Task<ServiceResponse<List<CartProductResponseDto>>> GetDbCartItems(int? userId = null)
        {
            userId ??= _authService.GetNameIdFromClaims();

            return await GetCartProductsAsync(await _context.CartItems
                .Where(ci => ci.UserId == userId).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = _authService.GetNameIdFromClaims();

            CartItem? sameItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
                ci.ProductTypeId == cartItem.ProductTypeId &&
                ci.UserId == cartItem.UserId);
            if (sameItem is null)
                _context.CartItems.Add(cartItem);
            else
                sameItem.Quantity += cartItem.Quantity;

            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true);
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            CartItem? dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
                ci.ProductTypeId == cartItem.ProductTypeId &&
                ci.UserId == _authService.GetNameIdFromClaims());
            if (dbCartItem is null)
                return new ServiceResponse<bool> { Error = "Cart item does not exist." };

            dbCartItem.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true);
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            CartItem? dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId &&
                ci.ProductTypeId == productTypeId &&
                ci.UserId == _authService.GetNameIdFromClaims());
            if (dbCartItem is null)
                return new ServiceResponse<bool> { Error = "Cart item does not exist." };

            _context.CartItems.Remove(dbCartItem);
            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true);
        }
    }
}