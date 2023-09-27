namespace BlazorEcommerce.Server.Services.CartService
{
    /// <summary>
    /// Implementation class of ICartService.
    /// </summary>
    public class CartService : ICartService
    {
        /// <summary>
        /// EcommerceContext field. Used to access the database context.
        /// </summary>
        private readonly EcommerceContext _context;
        /// <summary>
        /// IAuthService field. Used to access the Auth Services.
        /// </summary>
        private readonly IAuthService _authService;

        /// <param name="context">EcommerceContext instance to be passed on to the correct
        /// field, containing the correct implementation through the IoC container.</param>
        /// <param name="authService">IAuthService instance to be passed on to the correct
        /// field, containing the correct implementation class through the IoC container.</param>
        public CartService(EcommerceContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        /// <summary>
        /// Recieves the list of product dtos from a list of given cart items.
        /// </summary>
        /// <param name="cartItems">Represents the given list of cart items.</param>
        /// <returns>A list of CartProductsResponseDto on success,
        /// or an appropriate error message on failure.</returns>
        public async Task<ServiceResponse<List<CartProductResponseDto>>> GetCartProductsAsync(List<CartItem> cartItems)
        {
            List<CartProductResponseDto> response = new();

            foreach (CartItem item in cartItems)
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

        /// <summary>
        /// Saves the given list of cart items for the currently authenticated user to the database.
        /// </summary>
        /// <param name="cartItems">Represents the given list of cart items.</param>
        /// <returns>A list of CartProductResponseDto.</returns>
        public async Task<ServiceResponse<List<CartProductResponseDto>>> StoreCartItemsAsync(List<CartItem> cartItems)
        {
            cartItems.ForEach(cartItem => cartItem.UserId = _authService.GetNameIdFromClaims());
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetDbCartItems();
        }

        /// <summary>
        /// Get the amount of products in the cart of the currently authenticated user.
        /// </summary>
        /// <returns>An integer with the amount of cart products.</returns>
        public async Task<ServiceResponse<int>> GetCartItemsCountAsync() =>
            ServiceResponse<int>.SuccessResponse((await _context.CartItems
                .Where(ci => ci.UserId == _authService.GetNameIdFromClaims())
                .ToListAsync()).Count);

        /// <summary>
        /// Recieves a list of cart items for the user either with the given ID,
        /// or the currently authenticated users ID.
        /// </summary>
        /// <param name="userId">(Optional) Represents the given 
        /// user ID to recieve cart items from the database for.</param>
        /// <returns>A list of CartProductResponseDto.</returns>
        public async Task<ServiceResponse<List<CartProductResponseDto>>> GetDbCartItems(int? userId = null)
        {
            userId ??= _authService.GetNameIdFromClaims();

            return await GetCartProductsAsync(await _context.CartItems
                .Where(ci => ci.UserId == userId).ToListAsync());
        }

        /// <summary>
        /// Adds the given cart item to the currently authenticated users cart in the database,
        /// or increases the quantity number if it already is in the cart.
        /// </summary>
        /// <param name="cartItem">Represents the given cart item to add to the cart.</param>
        /// <returns>True/False depending on the response.</returns>
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

        /// <summary>
        /// Updates the quantity of an already existing (if any) product in the cart.
        /// </summary>
        /// <param name="cartItem">Represents the given cart item, used to update the quantity.</param>
        /// <returns>True/False depending on the response,
        /// or an appropriate error message in case of failure.</returns>
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

        /// <summary>
        /// Removes the item from the currently authenticated users cart
        /// which matches the given product ID and product type ID.
        /// </summary>
        /// <param name="productId">Represents the given product ID.</param>
        /// <param name="productTypeId">Represents the given product type ID.</param>
        /// <returns>True/False depending on the success,
        /// or an appropriate error message in case of failure.</returns>
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