namespace BlazorEcommerce.Server.Services.OrderService
{
    /// <summary>
    /// Implementation class of IOrderService.
    /// </summary>
    public class OrderService : IOrderService
    {
        #region Fields
        /// <summary>
        /// EcommerceContext field. Used to access the database context.
        /// </summary>
        private readonly EcommerceContext _context;
        /// <summary>
        /// ICartService field. Used to access the Cart Services.
        /// </summary>
        private readonly ICartService _cartService;
        /// <summary>
        /// IAuthService field. Used to access the Auth Services.
        /// </summary>
        private readonly IAuthService _authService;
        #endregion

        #region Constructor
        /// <param name="context">EcommerceContext instance to be passed on to the correct
        /// field, containing the correct implementation through the IoC container.</param>
        /// <param name="cartService">ICartService instance to be passed on to the correct
        /// field, containing the correct implementation class through the IoC container.</param>
        /// <param name="authService">IAuthService instance to be passed on to the correct
        /// field, containing the correct implementation class through the IoC container.</param>
        public OrderService(
            EcommerceContext context,
            ICartService cartService,
            IAuthService authService)
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Places an order for the user with the given ID. The order contains
        /// all the cart items the current user has. This method also deletes those
        /// database cart items after the order is placed.
        /// </summary>
        /// <param name="userId">Represents the ID of the user to have placed an order.</param>
        /// <returns>True/False depending on the response.</returns>
        public async Task<ServiceResponse<bool>> PlaceOrder(int userId)
        {
            List<CartProductResponseDto>? products = (await _cartService.GetDbCartItems(userId)).Data;
            decimal totalPrice = 0;
            products?.ForEach(product => totalPrice += product.Price * product.Quantity);

            List<OrderItem> orderItems = new();
            products?.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity
            }));

            Order order = new()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(_context.CartItems
                .Where(ci => ci.UserId == userId));

            await _context.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true);
        }

        /// <summary>
        /// Recieves a list of the details of an order with the given ID.
        /// </summary>
        /// <param name="orderId">Represents the given order ID to get the order details from.</param>
        /// <returns>OrderDetailsDto with all of the order details.</returns>
        public async Task<ServiceResponse<OrderDetailsDto>> GetOrderDetailsAsync(int orderId)
        {
            Order? order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Images)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .Where(o => o.UserId == _authService.GetNameIdFromClaims() && o.Id == orderId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();

            if (order is null)
                return ServiceResponse<OrderDetailsDto>.ErrorResponse("Order not found.");

            OrderDetailsDto orderDetailsResponse = new()
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new()
            };

            order.OrderItems.ForEach(item =>
            orderDetailsResponse.Products.Add(new OrderDetailsProductDto
            {
                // TODO Might be wise to add Automapper / Mapster here
                ProductId = item.ProductId,
                ImageUrl = item.Product.ImageUrl,
                Images = item.Product.Images,
                ProductType = item.ProductType.Name,
                Quantity = item.Quantity,
                Title = item.Product.Title,
                TotalPrice = item.TotalPrice
            }));

            return ServiceResponse<OrderDetailsDto>.SuccessResponse(orderDetailsResponse);
        }

        /// <summary>
        /// Recieves a list of all orders of the currently authenticated user.
        /// </summary>
        /// <returns>A list of OrderOverviewDto.</returns>
        public async Task<ServiceResponse<List<OrderOverviewDto>>> GetOrders()
        {
            List<Order> orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Images)
                .Where(o => o.UserId == _authService.GetNameIdFromClaims())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            List<OrderOverviewDto> orderResponse = new();
            orders.ForEach(o => orderResponse.Add(new OrderOverviewDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalPrice = o.TotalPrice,
                Product = o.OrderItems.Count > 1 ?
                    $"{o.OrderItems.First().Product.Title} and " +
                    $"{o.OrderItems.Count - 1} more..." :
                    o.OrderItems.First().Product.Title,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl,
                Images = o.OrderItems.First().Product.Images
            }));

            return ServiceResponse<List<OrderOverviewDto>>.SuccessResponse(orderResponse);
        } 
        #endregion
    }
}