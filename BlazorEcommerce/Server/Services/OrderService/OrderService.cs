﻿namespace BlazorEcommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly EcommerceContext _context;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(
            EcommerceContext context,
            ICartService cartService,
            IAuthService authService )
        {
            _context = context;
            _cartService = cartService;
            _authService = authService;
        }

        public async Task<ServiceResponse<OrderDetailsDto>> GetOrderDetailsAsync(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsDto>();
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Images)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .Where(o => o.UserId == _authService.GetNameIdFromClaims() && o.Id == orderId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();

            if (order is null)
            {
                response.Success = false;
                response.Message = "Order not found.";
                return response;
            }

            var orderDetailsResponse = new OrderDetailsDto
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductDto>()
            };

            order.OrderItems.ForEach(item =>
            orderDetailsResponse.Products.Add(new OrderDetailsProductDto
            {
                ProductId = item.ProductId,
                ImageUrl = item.Product.ImageUrl,
                Images = item.Product.Images,
                ProductType = item.ProductType.Name,
                Quantity = item.Quantity,
                Title = item.Product.Title,
                TotalPrice = item.TotalPrice
            }));

            response.Data = orderDetailsResponse;
            return response;
        }

        public async Task<ServiceResponse<List<OrderOverviewDto>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderOverviewDto>>();
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ThenInclude(p => p.Images)
                .Where(o => o.UserId == _authService.GetNameIdFromClaims())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            var orderResponse = new List<OrderOverviewDto>();
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

            response.Data = orderResponse;

            return response;
        }

        public async Task<ServiceResponse<bool>> PlaceOrder(int userId)
        {
            var products = (await _cartService.GetDbCartItems(userId)).Data;
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity
            }));

            var order = new Order
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

            return new ServiceResponse<bool>
            {
                Data = true
            };
        }
    }
}