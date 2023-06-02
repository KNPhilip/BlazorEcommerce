using BlazorEcommerce.Server.Migrations;
using BlazorEcommerce.Shared.Models;

namespace BlazorEcommerce.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly EcommerceContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(EcommerceContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<List<CartProductResponseDto>>> GetCartProductsAsync(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponseDto>>
            {
                Data = new List<CartProductResponseDto>()
            };

            foreach (var item in cartItems)
            {
                var product = await _context.Products
                    .Where(p => p.Id == item.ProductId)
                    .FirstOrDefaultAsync();

                if (product is null)
                    continue;

                var productVariant = await _context.ProductVariants
                    .Where(v => v.ProductId == item.ProductId && v.ProductTypeId == item.ProductTypeId)
                    .Include(v => v.ProductType)
                    .FirstOrDefaultAsync();

                if (productVariant is null)
                    continue;

                var cartProduct = new CartProductResponseDto
                {
                    ProductId = product.Id,
                    Title = product.Title,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductType = productVariant.ProductType.Name,
                    ProductTypeId = productVariant.ProductTypeId,
                    Quantity = item.Quantity
                };

                result.Data.Add(cartProduct);
            }

            return result;
        }

        public async Task<ServiceResponse<List<CartProductResponseDto>>> StoreCartItemsAsync(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = GetNameIdFromClaims());
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetDbCartItems();
        }

        public async Task<ServiceResponse<int>> GetCartItemsCountAsync()
        {
            var count = (await _context.CartItems.Where(ci => ci.UserId == GetNameIdFromClaims()).ToListAsync()).Count();
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<CartProductResponseDto>>> GetDbCartItems()
        {
            return await GetCartProductsAsync(await _context.CartItems
                .Where(ci => ci.UserId == GetNameIdFromClaims()).ToListAsync());
        }

        private int GetNameIdFromClaims() =>
            int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
        {
            cartItem.UserId = GetNameIdFromClaims();

            var sameItem = await _context.CartItems
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

            return new ServiceResponse<bool>
            {
                Data = true
            };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId &&
                ci.ProductTypeId == cartItem.ProductTypeId &&
                ci.UserId == GetNameIdFromClaims());
            if (dbCartItem is null)
            {
                return new ServiceResponse<bool> 
                { 
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist."
                };
            }

            dbCartItem.Quantity = cartItem.Quantity;
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true
            };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var dbCartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId.ProductId &&
                ci.ProductTypeId == productTypeId &&
                ci.UserId == GetNameIdFromClaims());
            if (dbCartItem is null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Cart item does not exist."
                };
            }

            _context.CartItems.Remove(dbCartItem);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> 
            { 
                Data = true 
            };
        }
    }
}