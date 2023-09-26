namespace BlazorEcommerce.Server.Services.CartService
{
    /// <summary>
    /// Interface for all things regarding Cart Services.
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Recieves the list of product dtos from a list of given cart items.
        /// </summary>
        /// <param name="cartItems"></param>
        /// <returns>A list of CartProductsResponseDto on success.</returns>
        Task<ServiceResponse<List<CartProductResponseDto>>> GetCartProductsAsync(List<CartItem> cartItems);

        /// <summary>
        /// Saves the given list of cart items for the currently authenticated user to the database.
        /// </summary>
        /// <param name="cartItems"></param>
        /// <returns>A list of CartProductResponseDto.</returns>
        Task<ServiceResponse<List<CartProductResponseDto>>> StoreCartItemsAsync(List<CartItem> cartItems);

        /// <summary>
        /// Get the amount of products in the cart of the currently authenticated user.
        /// </summary>
        /// <returns>An integer with the amount of cart products.</returns>
        Task<ServiceResponse<int>> GetCartItemsCountAsync();

        /// <summary>
        /// Recieves a list of cart items for the user either with the given ID,
        /// or the currently authenticated users ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A list of CartProductResponseDto.</returns>
        Task<ServiceResponse<List<CartProductResponseDto>>> GetDbCartItems(int? userId = null);

        /// <summary>
        /// Adds the given cart item to the currently authenticated users cart in the database,
        /// or increases the quantity number if it already is in the cart.
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns>True/False depending on the response.</returns>
        Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);

        /// <summary>
        /// Updates the quantity of an already existing (if any) product in the cart.
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns>True/False depending on the response.</returns>
        Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem);

        /// <summary>
        /// Removes the item from the currently authenticated users cart
        /// which matches the given product ID & product type ID.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productTypeId"></param>
        /// <returns>True/False depending on the success.</returns>
        Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId);
    }
}